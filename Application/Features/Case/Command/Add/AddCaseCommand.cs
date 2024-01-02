using Application.Dtos.Requests.Criminal;
using Application.Dtos.Requests.Evidence;
using Application.Dtos.Requests.Image;
using Application.Dtos.Requests.Testimony;
using Application.Dtos.Requests.Victim;
using Application.Dtos.Requests.WantedCriminal;
using Application.Dtos.Requests.Witness;
using Application.Interfaces.Account;
using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.CaseImage;
using Application.Interfaces.CaseInvestigator;
using Application.Interfaces.CaseVictim;
using Application.Interfaces.CaseWitness;
using Application.Interfaces.Criminal;
using Application.Interfaces.Evidence;
using Application.Interfaces.Repositories;
using Application.Interfaces.Victim;
using Application.Interfaces.WantedCriminal;
using Application.Interfaces.Witness;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Entities.Criminal;
using Domain.Entities.Witness;
using Domain.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Features.Case.Command.Add
{
    public class AddCaseCommand : IRequest<Result<AddCaseCommand>>
    {
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime StartDate { get; set; }
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime? EndDate { get; set; }
        public TypeOfViolation TypeOfViolation { get; set; }
        public CaseStatus Status { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_CHARGE)]
        [RegularExpression(@"^[\p{L} ,]+$", ErrorMessage = StaticVariable.CHARGE_VALID_CHARACTER)]
        public string Charge { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_CRIME_SCENE)]
        [RegularExpression(@"^[\p{L}0-9,. ]+$", ErrorMessage = StaticVariable.ADDRESS_VALID_CHARACTER)]
        public string Area { get; set; } = null!;
        public string? Description { get; set; }
        public List<EvidenceRequest>? Evidences { get; set; }
        public List<WitnessRequest> Witnesses { get; set; } = null!;
        public List<ImageRequest>? CaseImage { get; set; }
        public List<CriminalRequest>? Criminals { get; set; }
        public List<long>? InvestigatorIds { get; set; }
        public List<VictimRequest>? Victims { get; set; }
        public List<WantedCriminalRequest>? WantedCriminalRequest { get; set; }
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
        private readonly IWantedCriminalRepository _wantedCriminalRepository;
        private readonly IEvidenceRepository _evidenceRepository;
        private readonly IMapper _mapper;
        public AddCaseCommandHandler(IUnitOfWork<long> unitOfWork, ICaseRepository caseRepository,
            ICaseImageRepository caseImageRepository, ICaseCriminalRepository caseCriminalRepository,
            IWitnessRepository witnessRepository, ICriminalRepository criminalRepository,
            ICaseWitnessRepository caseWitnessRepository, ICaseInvestigatorRepository caseInvestigatorRepository,
            IAccountRepository accountRepository, IVictimRepository victimRepository,
            ICaseVictimRepository caseVictimRepository,
            IWantedCriminalRepository wantedCriminalRepository,
            IEvidenceRepository evidenceRepository,
            IMapper mapper)
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
            _wantedCriminalRepository = wantedCriminalRepository;
            _evidenceRepository = evidenceRepository;
            _mapper = mapper;
        }
        public async Task<Result<AddCaseCommand>> Handle(AddCaseCommand request, CancellationToken cancellationToken)
        {
            if (request.InvestigatorIds != null && request.InvestigatorIds.Count > 0)
            {
                foreach (var id in request.InvestigatorIds)
                {
                    var checkInvestigatorExist = _accountRepository.Entities.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
                    if (checkInvestigatorExist == null)
                    {
                        return await Result<AddCaseCommand>.FailAsync(StaticVariable.NOT_FOUND_INVESTIGATOR);
                    }
                }
            }
            List<Domain.Entities.CaseCriminal.CaseCriminal> caseCriminals = new List<Domain.Entities.CaseCriminal.CaseCriminal>();
            if (request.Criminals != null && request.Criminals.Count > 0)
            {
                foreach (var criminal in request.Criminals)
                {
                    var isCriminalExists = _criminalRepository.Entities.Where(_ => _.Id == criminal.Id && !_.IsDeleted).FirstOrDefault();
                    if (isCriminalExists == null)
                    {
                        return await Result<AddCaseCommand>.FailAsync(StaticVariable.NOT_FOUND_CRIMINAL);
                    }
                    caseCriminals.Add(new Domain.Entities.CaseCriminal.CaseCriminal
                    {
                        CaseId = 0,
                        CriminalId = criminal.Id,
                        Testimony = criminal.Testimony,
                        Date = criminal.Date, 
                        Charge = criminal.Charge,
                        Reason = criminal.Reason,
                        Weapon = criminal.Weapon,
                        TypeOfViolation = criminal.TypeOfViolation
                    });
                }
            }
            var addCase = _mapper.Map<Domain.Entities.Case.Case>(request);
            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _caseRepository.AddAsync(addCase);
                    await _unitOfWork.Commit(cancellationToken);
                    if (request.Criminals != null && request.Criminals.Count > 0)
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
                        List<TestimonyRequest> witnessTestimonies = new List<TestimonyRequest>();
                        foreach (var witness in request.Witnesses)
                        {
                            var checkWitnessExist = _witnessRepository.Entities.Where(_ => _.CitizenId.Equals(witness.CitizenId)).FirstOrDefault();
                            if (checkWitnessExist == null)
                            {
                                if (!string.IsNullOrEmpty(witness.PhoneNumber))
                                {
                                    var checkPhoneWitnessExist = _witnessRepository.Entities.Where(_ => _.PhoneNumber.Equals(witness.PhoneNumber) && !_.IsDeleted).FirstOrDefault();
                                    if (checkPhoneWitnessExist != null) return await Result<AddCaseCommand>.FailAsync(StaticVariable.PHONE_NUMBER_WITNESS_EXISTS_MSG);
                                }
                                var addWitness = _mapper.Map<Domain.Entities.Witness.Witness>(witness);

                                await _witnessRepository.AddAsync(addWitness);
                                await _unitOfWork.Commit(cancellationToken);
                                witnessTestimonies.Add(new TestimonyRequest
                                {
                                    Id = addWitness.Id,
                                    Testimony = witness.Testimony,
                                    Date = witness.Date
                                });
                            }
                            else
                            {
                                witnessTestimonies.Add(new TestimonyRequest
                                {
                                    Id = checkWitnessExist.Id,
                                    Testimony = witness.Testimony,
                                    Date = witness.Date
                                });
                            }
                        }
                        List<Domain.Entities.CaseWitness.CaseWitness> caseWitnesses = new List<Domain.Entities.CaseWitness.CaseWitness>();
                        foreach (var witnessTestimony in witnessTestimonies)
                        {
                            caseWitnesses.Add(new Domain.Entities.CaseWitness.CaseWitness
                            {
                                CaseId = addCase.Id,
                                WitnessId = witnessTestimony.Id,
                                Testimony = witnessTestimony.Testimony,
                                Date = witnessTestimony.Date
                            });
                        }
                        await _caseWitnessRepository.AddRangeAsync(caseWitnesses);
                    }
                    if (request.Victims != null && request.Victims.Count > 0)
                    {
                        List<TestimonyRequest> victimTestimonies = new List<TestimonyRequest>();
                        foreach (var victim in request.Victims)
                        {
                            var checkVictimExist = _victimRepository.Entities.Where(_ => _.CitizenId.Equals(victim.CitizenId) && !_.IsDeleted).FirstOrDefault();
                            if (checkVictimExist == null)
                            {
                                if (!string.IsNullOrEmpty(victim.PhoneNumber))
                                {
                                    var checkPhoneVictimExist = _victimRepository.Entities.Where(_ => _.PhoneNumber.Equals(victim.PhoneNumber) && !_.IsDeleted).FirstOrDefault();
                                    if (checkPhoneVictimExist != null) return await Result<AddCaseCommand>.FailAsync(StaticVariable.PHONE_NUMBER_VICTIM_EXISTS_MSG);
                                }
                                var addVictim = _mapper.Map<Domain.Entities.Victim.Victim>(victim);
                                await _victimRepository.AddAsync(addVictim);
                                await _unitOfWork.Commit(cancellationToken);
                                victimTestimonies.Add(new TestimonyRequest
                                {
                                    Id = addVictim.Id,
                                    Testimony = victim.Testimony,
                                    Date = victim.Date
                                });
                            }
                            else
                            {
                                victimTestimonies.Add(new TestimonyRequest
                                {
                                    Id = checkVictimExist.Id,
                                    Testimony = victim.Testimony,
                                    Date = victim.Date
                                });
                            }
                        }
                        List<Domain.Entities.CaseVictim.CaseVictim> caseVictims = new List<Domain.Entities.CaseVictim.CaseVictim>();
                        foreach (var victimTestimony in victimTestimonies)
                        {
                            caseVictims.Add(new Domain.Entities.CaseVictim.CaseVictim
                            {
                                CaseId = addCase.Id,
                                VictimId = victimTestimony.Id,
                                Testimony = victimTestimony.Testimony,
                                Date = victimTestimony.Date
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
                    if (request.WantedCriminalRequest != null && request.WantedCriminalRequest.Count > 0)
                    {
                        var wantedCriminalRequest = _mapper.Map<List<Domain.Entities.WantedCriminal.WantedCriminal>>(request.WantedCriminalRequest);
                        wantedCriminalRequest.ForEach(x => x.CaseId = addCase.Id);
                        await _wantedCriminalRepository.AddRangeAsync(wantedCriminalRequest);
                    }
                    if (request.Evidences != null && request.Evidences.Count > 0)
                    {
                        foreach (var evidence in request.Evidences)
                        {
                            var addEvidence = _mapper.Map<Domain.Entities.Evidence.Evidence>(evidence);
                            addEvidence.CaseId = addCase.Id;
                            await _evidenceRepository.AddAsync(addEvidence);
                            await _unitOfWork.Commit(cancellationToken);
                            if (evidence.EvidenceImages != null && evidence.EvidenceImages.Count > 0)
                            {
                                var addEvidenceImage = _mapper.Map<List<Domain.Entities.CaseImage.CaseImage>>(evidence.EvidenceImages);
                                addEvidenceImage.ForEach(x =>
                                {
                                    x.CaseId = addCase.Id;
                                    x.EvidenceId = addEvidence.Id;
                                });
                                await _caseImageRepository.AddRangeAsync(addEvidenceImage);
                            }
                        }
                    }
                    await _unitOfWork.Commit(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return await Result<AddCaseCommand>.SuccessAsync(request);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return await Result<AddCaseCommand>.FailAsync(ex.Message);
                }
                finally
                {
                    await transaction.DisposeAsync();
                }
            });

            return result;
        }
    }
}
