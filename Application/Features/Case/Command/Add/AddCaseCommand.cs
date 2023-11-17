using Application.Dtos.Requests.Evidence;
using Application.Dtos.Requests.Image;
using Application.Dtos.Requests.Victim;
using Application.Dtos.Requests.Witness;
using Application.Interfaces.Account;
using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.CaseImage;
using Application.Interfaces.CaseInvestigator;
using Application.Interfaces.CaseVictim;
using Application.Interfaces.CaseWitness;
using Application.Interfaces.Criminal;
using Application.Interfaces.Repositories;
using Application.Interfaces.Victim;
using Application.Interfaces.Witness;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Wrappers;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Features.Case.Command.Add
{
    public class AddCaseCommand : IRequest<Result<AddCaseCommand>>
    {
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_REASON)]
        public string? Reason { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_MURDER_WEAPON)]
        public string? MurderWeapon { get; set; }
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime StartDate { get; set; }
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime? EndDate { get; set; }
        public TypeOfViolation TypeOfViolation { get; set; }
        public short Status { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_CHARGE)]
        public string Charge { get; set; } = null!;
        public List<EvidenceRequest>? Evidences { get; set; }
        public List<WitnessRequest> Witnesses { get; set; }
        public List<ImageRequest>? CaseImage { get; set; }
        public List<long>? CriminalIds { get; set; }
        public List<long>? InvestigatorIds { get; set; }
        public List<VictimRequest>? Victims { get; set; }
    }
    internal class AddCaseCommandHandler : IRequestHandler<AddCaseCommand, Result<AddCaseCommand>>
    {
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseImageRepository _caseImageRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        private readonly IWitnessRepository _witnessRepository;
        private readonly ICriminalRepository _criminalRepository;
        private readonly ICaseWitnessRepository _caseWitnessRepository;
        private readonly ICaseInvestigatorRepository _caseInvestigatorRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IVictimRepository _victimRepository;
        private readonly ICaseVictimRepository _caseVictimRepository;
        private readonly IMapper _mapper;
        public AddCaseCommandHandler(IUnitOfWork<long> unitOfWork, ICaseRepository caseRepository,
            ICaseImageRepository caseImageRepository, ICaseCriminalRepository caseCriminalRepository,
            IWitnessRepository witnessRepository, ICriminalRepository criminalRepository,
            ICaseWitnessRepository caseWitnessRepository, ICaseInvestigatorRepository caseInvestigatorRepository,
            IAccountRepository accountRepository, IVictimRepository victimRepository,
            ICaseVictimRepository caseVictimRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _caseRepository = caseRepository;
            _caseImageRepository = caseImageRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _witnessRepository = witnessRepository;
            _criminalRepository = criminalRepository;
            _caseWitnessRepository = caseWitnessRepository;
            _caseInvestigatorRepository = caseInvestigatorRepository;
            _accountRepository = accountRepository;
            _victimRepository = victimRepository;
            _caseVictimRepository = caseVictimRepository;
            _mapper = mapper;
        }
        public async Task<Result<AddCaseCommand>> Handle(AddCaseCommand request, CancellationToken cancellationToken)
        {
            if (request.InvestigatorIds != null && request.InvestigatorIds.Count > 0)
            {
                foreach (var id in request.InvestigatorIds)
                {
                    var checkInvestigatorExist = await _accountRepository.FindAsync(_ => _.Id == id && !_.IsDeleted);
                    if (checkInvestigatorExist == null)
                    {
                        return await Result<AddCaseCommand>.FailAsync(StaticVariable.NOT_FOUND_INVESTIGATOR);
                    }
                }
            }
            List<Domain.Entities.CaseCriminal.CaseCriminal> caseCriminals = new List<Domain.Entities.CaseCriminal.CaseCriminal>();
            if (request.CriminalIds != null && request.CriminalIds.Count > 0)
            {
                foreach (var criminalId in request.CriminalIds)
                {
                    var isCriminalExists = await _criminalRepository.FindAsync(_ => _.Id == criminalId);
                    if (isCriminalExists == null)
                    {
                        return await Result<AddCaseCommand>.FailAsync(StaticVariable.NOT_FOUND_CRIMINAL);
                    }
                    caseCriminals.Add(new Domain.Entities.CaseCriminal.CaseCriminal
                    {
                        CaseId = 0,
                        CriminalId = criminalId
                    });
                }
            }
            var addCase = _mapper.Map<Domain.Entities.Case.Case>(request);
            await _caseRepository.AddAsync(addCase);
            await _unitOfWork.Commit(cancellationToken);
            if (request.CriminalIds != null && request.CriminalIds.Count > 0)
            {
                caseCriminals.ForEach(_ => _.CaseId = addCase.Id);
                await _caseCriminalRepository.AddRangeAsync(caseCriminals);
            }
            if (request.CaseImage != null && request.CaseImage.Count > 0)
            {
                var addImage = _mapper.Map<List<Domain.Entities.CaseImage.CaseImage>>(request.CaseImage);
                addImage.ForEach(x => x.CaseId = addCase.Id);
                await _caseImageRepository.AddRangeAsync(addImage);
            }
            // await _unitOfWork.Commit(cancellationToken);
            if (request.Witnesses != null && request.Witnesses.Count > 0)
            {
                List<Domain.Entities.Witness.Witness> addWitnesses = new List<Domain.Entities.Witness.Witness>();
                List<long> witnessIds = new List<long>();
                foreach (var witness in request.Witnesses)
                {
                    var checkWitnessExist = await _witnessRepository.FindAsync(_ => _.CitizenID.Equals(witness.CitizenID));
                    if (checkWitnessExist == null)
                    {
                        var addWitness = _mapper.Map<Domain.Entities.Witness.Witness>(witness);
                        addWitnesses.Add(addWitness);
                    }
                    else
                    {
                        witnessIds.Add(checkWitnessExist.Id);
                    }
                }
                if (addWitnesses.Count > 0)
                {
                    await _witnessRepository.AddRangeAsync(addWitnesses);
                    await _unitOfWork.Commit(cancellationToken);
                    foreach (var witness in addWitnesses)
                    {
                        witnessIds.Add(witness.Id);
                    }
                }
                List<Domain.Entities.CaseWitness.CaseWitness> caseWitnesses = new List<Domain.Entities.CaseWitness.CaseWitness>();
                foreach (var id in witnessIds)
                {
                    caseWitnesses.Add(new Domain.Entities.CaseWitness.CaseWitness
                    {
                        CaseId = addCase.Id,
                        WitnessId = id
                    });
                }
                await _caseWitnessRepository.AddRangeAsync(caseWitnesses);
            }
            if (request.Victims != null && request.Victims.Count > 0)
            {
                List<Domain.Entities.Victim.Victim> addVictims = new List<Domain.Entities.Victim.Victim>();
                List<long> victimIds = new List<long>();
                foreach (var victim in request.Victims)
                {
                    var checkVictimExist = await _victimRepository.FindAsync(_ => _.CitizenID.Equals(victim.CitizenID) && !_.IsDeleted);
                    if (checkVictimExist == null)
                    {
                        var addVictim = _mapper.Map<Domain.Entities.Victim.Victim>(victim);
                        addVictims.Add(addVictim);
                    }
                    else
                    {
                        victimIds.Add(checkVictimExist.Id);
                    }
                }
                if (addVictims.Count > 0)
                {
                    await _victimRepository.AddRangeAsync(addVictims);
                    await _unitOfWork.Commit(cancellationToken);
                    foreach (var victim in addVictims)
                    {
                        victimIds.Add(victim.Id);
                    }
                }
                List<Domain.Entities.CaseVictim.CaseVictim> caseVictims = new List<Domain.Entities.CaseVictim.CaseVictim>();
                foreach (var id in victimIds)
                {
                    caseVictims.Add(new Domain.Entities.CaseVictim.CaseVictim
                    {
                        CaseId = addCase.Id,
                        VictimId = id
                    });
                }
                await _caseVictimRepository.AddRangeAsync(caseVictims);
            }
            if (request.InvestigatorIds != null && request.InvestigatorIds.Count > 0)
            {
                foreach (var id in request.InvestigatorIds)
                {
                    await _caseInvestigatorRepository.AddAsync(new Domain.Entities.CaseInvestigator.CaseInvestigator
                    {
                        CaseId = addCase.Id,
                        InvestigatorId = id
                    });
                }
            }
            await _unitOfWork.Commit(cancellationToken);
            return await Result<AddCaseCommand>.SuccessAsync(request);
        }
    }
}
