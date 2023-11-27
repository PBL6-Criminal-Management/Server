using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.Dtos.Requests.Evidence;
using Application.Dtos.Requests.Image;
using Application.Dtos.Requests.Victim;
using Application.Dtos.Requests.Witness;
using Application.Features.Account.Command.Edit;
using Application.Interfaces;
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
using Application.Interfaces.Witness;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Case.Command.Edit
{
    public class EditCaseCommand : IRequest<Result<EditCaseCommand>>
    {
        public long Id { get; set; }
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_REASON)]
        public string? Reason { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_MURDER_WEAPON)]
        public string? MurderWeapon { get; set; }
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime StartDate { get; set; }
        [JsonConverter(typeof(CustomConverter.DateTimeConverter))]
        public DateTime? EndDate { get; set; }
        public TypeOfViolation TypeOfViolation { get; set; }
        public CaseStatus Status { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_CHARGE)]
        public string Charge { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_CRIME_SCENE)]
        public string CrimeScene { get; set; } = null!;
        public List<EvidenceRequest>? Evidences { get; set; }
        public List<WitnessRequest> Witnesses { get; set; } = null!;
        public List<ImageRequest>? CaseImage { get; set; }
        public List<long>? CriminalIds { get; set; }
        public List<long>? InvestigatorIds { get; set; }
        public List<VictimRequest>? Victims { get; set; }
    }
    internal class EditCaseCommandHandler : IRequestHandler<EditCaseCommand, Result<EditCaseCommand>>
    {
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseImageRepository _caseImageRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        private readonly IWitnessRepository _witnessRepository;
        private readonly ICriminalRepository _criminalRepository;
        private readonly ICaseWitnessRepository _caseWitnessRepository;
        private readonly ICaseInvestigatorRepository _caseInvestigatorRepository;
        private readonly IVictimRepository _victimRepository;
        private readonly ICaseVictimRepository _caseVictimRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUploadService _uploadService;
        private readonly IEvidenceRepository _evidenceRepository;
        private readonly IMapper _mapper;
        public EditCaseCommandHandler(IUnitOfWork<long> unitOfWork, ICaseRepository caseRepository,
            ICaseImageRepository caseImageRepository, ICaseCriminalRepository caseCriminalRepository,
            IWitnessRepository witnessRepository, ICriminalRepository criminalRepository,
            ICaseWitnessRepository caseWitnessRepository, ICaseInvestigatorRepository caseInvestigatorRepository,
            IAccountRepository accountRepository, IVictimRepository victimRepository,
            ICaseVictimRepository caseVictimRepository, IUploadService uploadService,
            IEvidenceRepository evidenceRepository, IMapper mapper)
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
            _uploadService = uploadService;
            _evidenceRepository = evidenceRepository;
            _mapper = mapper;
        }
        public async Task<Result<EditCaseCommand>> Handle(EditCaseCommand request, CancellationToken cancellationToken)
        {
            var editCase = await _caseRepository.FindAsync(_ => _.Id == request.Id && !_.IsDeleted);
            if (editCase == null)
            {
                return await Result<EditCaseCommand>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }
            List<Domain.Entities.CaseCriminal.CaseCriminal> newCaseCriminals = new List<Domain.Entities.CaseCriminal.CaseCriminal>();
            if (request.CriminalIds != null && request.CriminalIds.Count > 0)
            {
                foreach (var criminalId in request.CriminalIds)
                {
                    var isCriminalExists = await _criminalRepository.FindAsync(_ => _.Id == criminalId);
                    if (isCriminalExists == null)
                    {
                        return await Result<EditCaseCommand>.FailAsync(StaticVariable.NOT_FOUND_CRIMINAL);
                    }
                    newCaseCriminals.Add(new Domain.Entities.CaseCriminal.CaseCriminal
                    {
                        CaseId = request.Id,
                        CriminalId = criminalId
                    });
                }
            }
            List<Domain.Entities.CaseInvestigator.CaseInvestigator> newCaseInvestigators = new List<Domain.Entities.CaseInvestigator.CaseInvestigator>();
            if (request.InvestigatorIds != null && request.InvestigatorIds.Count > 0)
            {
                foreach (var investigatorId in request.InvestigatorIds)
                {
                    var checkInvestigatorExist = await _accountRepository.FindAsync(_ => _.Id == investigatorId && !_.IsDeleted);
                    if (checkInvestigatorExist == null)
                    {
                        return await Result<EditCaseCommand>.FailAsync(StaticVariable.NOT_FOUND_INVESTIGATOR);
                    }
                    newCaseInvestigators.Add(new Domain.Entities.CaseInvestigator.CaseInvestigator
                    {
                        CaseId = request.Id,
                        InvestigatorId = investigatorId
                    });
                }
            }
            _mapper.Map(request, editCase);

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            var result = await executionStrategy.ExecuteAsync(async () =>
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _caseRepository.UpdateAsync(editCase);
                    await _unitOfWork.Commit(cancellationToken);
                    var caseEvidences = await _evidenceRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                    if (request.Evidences != null && request.Evidences.Count > 0)
                    {
                        foreach (var evidence in request.Evidences)
                        {
                            if (evidence.Id == 0)
                            {
                                var addEvidence = _mapper.Map<Domain.Entities.Evidence.Evidence>(evidence);
                                addEvidence.CaseId = request.Id;
                                await _evidenceRepository.AddAsync(addEvidence);
                            }
                            else
                            {
                                foreach (var caseEvidence in caseEvidences)
                                {
                                    if (caseEvidence.Id == evidence.Id)
                                    {
                                        caseEvidences.Remove(caseEvidence);
                                        _mapper.Map(evidence, caseEvidence);
                                        await _evidenceRepository.UpdateAsync(caseEvidence);
                                        break;
                                    }
                                }
                            }
                        }
                        if (caseEvidences.Count > 0)
                        {
                            await _evidenceRepository.DeleteRange(caseEvidences);
                        }
                    }
                    else
                    {
                        if (caseEvidences != null)
                        {
                            await _evidenceRepository.DeleteRange(caseEvidences);
                        }
                    }
                    var caseCriminals = await _caseCriminalRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                    if (request.CriminalIds != null && request.CriminalIds.Count > 0)
                    {
                        foreach (var caseCriminal in caseCriminals)
                        {
                            bool check = false;
                            foreach (var newCaseCriminal in newCaseCriminals)
                            {
                                if (newCaseCriminal.CriminalId == caseCriminal.CriminalId)
                                {
                                    check = true;
                                    newCaseCriminals.Remove(newCaseCriminal);
                                    break;
                                }
                            }
                            if (check == false)
                            {
                                await _caseCriminalRepository.DeleteAsync(caseCriminal);
                            }
                        }
                        if (newCaseCriminals.Count > 0)
                        {
                            await _caseCriminalRepository.AddRangeAsync(newCaseCriminals);
                        }
                    }
                    else
                    {
                        if (caseCriminals != null)
                        {
                            await _caseCriminalRepository.DeleteRange(caseCriminals);
                        }
                    }
                    var caseInvestigators = await _caseInvestigatorRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                    if (request.InvestigatorIds != null && request.InvestigatorIds.Count > 0)
                    {
                        foreach (var caseInvestigator in caseInvestigators)
                        {
                            bool check = false;
                            foreach (var newCaseInvestigator in newCaseInvestigators)
                            {
                                if (newCaseInvestigator.InvestigatorId == caseInvestigator.InvestigatorId)
                                {
                                    check = true;
                                    newCaseInvestigators.Remove(newCaseInvestigator);
                                    break;
                                }
                            }
                            if (!check)
                            {
                                await _caseInvestigatorRepository.DeleteAsync(caseInvestigator);
                            }
                        }
                        if (newCaseInvestigators.Count > 0)
                        {
                            await _caseInvestigatorRepository.AddRangeAsync(caseInvestigators);
                        }
                    }
                    else
                    {
                        if (caseInvestigators != null)
                        {
                            await _caseInvestigatorRepository.DeleteRange(caseInvestigators);
                        }
                    }
                    var caseWitnesses = await _caseWitnessRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                    if (request.Witnesses != null && request.Witnesses.Count > 0)
                    {
                        foreach (var witness in request.Witnesses)
                        {
                            var checkWitnessExist = await _witnessRepository.FindAsync(_ => _.Id == witness.Id && !_.IsDeleted);
                            if (checkWitnessExist == null || witness.Id == 0)
                            {
                                var checkCitizenIdExist = await _witnessRepository.FindAsync(_ => _.CitizenId.Equals(witness.CitizenId));
                                if (checkCitizenIdExist == null)
                                {
                                    var addWitness = _mapper.Map<Domain.Entities.Witness.Witness>(witness);
                                    await _witnessRepository.AddAsync(addWitness);
                                    await _unitOfWork.Commit(cancellationToken);
                                    await _caseWitnessRepository.AddAsync(new Domain.Entities.CaseWitness.CaseWitness
                                    {
                                        CaseId = request.Id,
                                        WitnessId = addWitness.Id
                                    });
                                }
                                else
                                {
                                    await _caseWitnessRepository.AddAsync(new Domain.Entities.CaseWitness.CaseWitness
                                    {
                                        CaseId = request.Id,
                                        WitnessId = checkCitizenIdExist.Id
                                    });
                                }
                            }
                            else
                            {
                                bool check = false;
                                foreach (var caseWitness in caseWitnesses)
                                {
                                    if (caseWitness.WitnessId == checkWitnessExist.Id)
                                    {
                                        check = true;
                                        _mapper.Map(witness, checkWitnessExist);
                                        await _witnessRepository.UpdateAsync(checkWitnessExist);
                                        caseWitnesses.Remove(caseWitness);
                                        break;
                                    }
                                }
                                if (!check)
                                {
                                    await _caseWitnessRepository.AddAsync(new Domain.Entities.CaseWitness.CaseWitness
                                    {
                                        CaseId = request.Id,
                                        WitnessId = checkWitnessExist.Id
                                    });
                                }
                            }
                        }
                        if (caseWitnesses.Count > 0)
                        {
                            await _caseWitnessRepository.DeleteRange(caseWitnesses);
                        }
                    }
                    else
                    {
                        if (caseWitnesses != null)
                        {
                            await _caseWitnessRepository.DeleteRange(caseWitnesses);
                        }
                    }
                    var caseVictims = await _caseVictimRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
                    if (request.Victims != null && request.Victims.Count > 0)
                    {
                        foreach (var victim in request.Victims)
                        {
                            var checkVictimExist = await _victimRepository.FindAsync(_ => _.Id == victim.Id && !_.IsDeleted);
                            if (checkVictimExist == null || victim.Id == 0)
                            {
                                var checkCitizenIdExist = await _victimRepository.FindAsync(_ => _.CitizenId.Equals(victim.CitizenId));
                                if (checkCitizenIdExist == null)
                                {
                                    var addVictim = _mapper.Map<Domain.Entities.Victim.Victim>(victim);
                                    await _victimRepository.AddAsync(addVictim);
                                    await _unitOfWork.Commit(cancellationToken);
                                    await _caseVictimRepository.AddAsync(new Domain.Entities.CaseVictim.CaseVictim
                                    {
                                        CaseId = request.Id,
                                        VictimId = addVictim.Id
                                    });
                                }
                                else
                                {
                                    await _caseVictimRepository.AddAsync(new Domain.Entities.CaseVictim.CaseVictim
                                    {
                                        CaseId = request.Id,
                                        VictimId = checkCitizenIdExist.Id
                                    });
                                }
                            }
                            else
                            {
                                bool check = false;
                                foreach (var caseVictim in caseVictims)
                                {
                                    if (caseVictim.VictimId == checkVictimExist.Id)
                                    {
                                        check = true;
                                        _mapper.Map(victim, checkVictimExist);
                                        await _victimRepository.UpdateAsync(checkVictimExist);
                                        caseVictims.Remove(caseVictim);
                                        break;
                                    }
                                }
                                if (!check)
                                {
                                    await _caseVictimRepository.AddAsync(new Domain.Entities.CaseVictim.CaseVictim
                                    {
                                        CaseId = request.Id,
                                        VictimId = checkVictimExist.Id
                                    });
                                }
                            }
                        }
                        if (caseVictims.Count > 0)
                        {
                            await _caseVictimRepository.DeleteRange(caseVictims);
                        }
                    }
                    else
                    {
                        if (caseVictims != null)
                        {
                            await _caseVictimRepository.DeleteRange(caseVictims);
                        }
                    }

                    var caseImagesInDb = await _caseImageRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();

                    if (request.CaseImage != null && request.CaseImage.Count > 0)
                    {
                        var images = _mapper.Map<List<Domain.Entities.CaseImage.CaseImage>>(request.CaseImage);
                        var requestImage = images.Select(_ =>
                        {
                            _.Id = 0;
                            _.CaseId = request.Id;
                            return _;
                        }).ToList();
                        await _caseImageRepository.AddRangeAsync(requestImage);
                        await _caseImageRepository.RemoveRangeAsync(caseImagesInDb);
                        await _unitOfWork.Commit(cancellationToken);

                        List<string> listNewFile = request.CaseImage.Select(_ => _.FilePath).ToList();

                        if (caseImagesInDb.Any() && listNewFile.Any())
                        {
                            foreach (var image in caseImagesInDb)
                            {
                                if (!listNewFile.Contains(image.FilePath)) await _uploadService.DeleteAsync(image.FilePath);
                            }
                        }
                    }
                    else
                    {
                        await _uploadService.DeleteRangeAsync(caseImagesInDb.Select(c => c.FilePath).ToList());
                    }
                    await _unitOfWork.Commit(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return await Result<EditCaseCommand>.SuccessAsync(request);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return await Result<EditCaseCommand>.FailAsync(ex.Message);
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