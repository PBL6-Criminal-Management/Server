using Application.Dtos.Responses.File;
using Application.Dtos.Responses.WantedCriminal;
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
using Application.Interfaces.Victim;
using Application.Interfaces.WantedCriminal;
using Application.Interfaces.Witness;
using AutoMapper;
using Domain.Constants;
using Domain.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Case.Queries.GetById
{
    public class GetCaseByIdQuery : IRequest<Result<GetCaseByIdResponse>>
    {
        public long Id { get; set; }
    }
    internal class GetCaseByIdQueryHandler : IRequestHandler<GetCaseByIdQuery, Result<GetCaseByIdResponse>>
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        private readonly ICriminalRepository _criminalRepository;
        private readonly IEvidenceRepository _evidenceRepository;
        private readonly ICaseWitnessRepository _caseWitnessRepository;
        private readonly IWitnessRepository _witnessRepository;
        private readonly ICaseImageRepository _caseImageRepository;
        private readonly IUploadService _uploadService;
        private readonly ICaseInvestigatorRepository _caseInvestigatorRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICaseVictimRepository _caseVictimRepository;
        private readonly IVictimRepository _victimRepository;
        private readonly IWantedCriminalRepository _wantedCriminalRepository;
        private readonly IMapper _mapper;

        public GetCaseByIdQueryHandler(ICaseRepository caseRepository, ICaseCriminalRepository caseCriminalRepository,
            ICriminalRepository criminalRepository, IEvidenceRepository evidenceRepository,
            ICaseWitnessRepository caseWitnessRepository, IWitnessRepository witnessRepository,
            ICaseImageRepository caseImageRepository, IUploadService uploadService,
            ICaseInvestigatorRepository caseInvestigatorRepository, IAccountRepository accountRepository,
            ICaseVictimRepository caseVictimRepository, IVictimRepository victimRepository,
            IWantedCriminalRepository wantedCriminalRepository,
            IMapper mapper)
        {
            _caseRepository = caseRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _criminalRepository = criminalRepository;
            _evidenceRepository = evidenceRepository;
            _caseWitnessRepository = caseWitnessRepository;
            _witnessRepository = witnessRepository;
            _caseImageRepository = caseImageRepository;
            _uploadService = uploadService;
            _caseInvestigatorRepository = caseInvestigatorRepository;
            _accountRepository = accountRepository;
            _caseVictimRepository = caseVictimRepository;
            _victimRepository = victimRepository;
            _wantedCriminalRepository = wantedCriminalRepository;
            _mapper = mapper;
        }
        public async Task<Result<GetCaseByIdResponse>> Handle(GetCaseByIdQuery request, CancellationToken cancellationToken)
        {
            var caseCheck = _caseRepository.Entities.Where(_ => _.Id == request.Id && !_.IsDeleted).FirstOrDefault();
            if (caseCheck == null)
            {
                return await Result<GetCaseByIdResponse>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }
            var response = _mapper.Map<GetCaseByIdResponse>(caseCheck);
            response.Code = StaticVariable.CASE + response.Id.ToString().PadLeft(5, '0');

            var evidenceOfCase = await _evidenceRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
            if (evidenceOfCase.Any()) response.Evidences = _mapper.Map<List<Dtos.Responses.Evidence.EvidenceResponse>>(evidenceOfCase).ToList();
            if (response.Evidences != null && response.Evidences.Count > 0)
            {
                foreach (var evidence in response.Evidences)
                {
                    var imageOfEvidence = await _caseImageRepository.Entities.Where(_ => _.CaseId == request.Id && _.EvidenceId == evidence.Id && !_.IsDeleted).ToListAsync();
                    evidence.EvidenceImages = imageOfEvidence.Select(i => new FileResponse
                    {
                        FileName = i.FileName,
                        FilePath = i.FilePath,
                        FileUrl = _uploadService.GetFullUrl(i.FilePath)
                    }).ToList();
                }
            }
            var witnessOfCase = await _caseWitnessRepository.Entities.Where(_ => _.CaseId == request.Id)
            .Join(_witnessRepository.Entities,
                cW => cW.WitnessId,
                w => w.Id,
                (cw, c) => new Dtos.Responses.Witness.WitnessResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    CitizenId = c.CitizenId,
                    Gender = c.Gender,
                    Birthday = c.Birthday,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    Date = cw.Date,
                    Testimony = cw.Testimony
                }).ToListAsync();
            if (witnessOfCase.Any()) response.Witnesses = witnessOfCase;
            var investigatorOFCase = await _caseInvestigatorRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted)
            .Join(_accountRepository.Entities,
                cI => cI.InvestigatorId,
                i => i.Id,
                (ci, i) => new Dtos.Responses.User.UserResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    Birthday = i.Birthday,
                    Gender = i.Gender,
                    PhoneNumber = i.PhoneNumber,
                    Address = i.Address
                }).ToListAsync();
            if (investigatorOFCase.Any()) response.Investigators = investigatorOFCase;
            var victimOfCase = await _caseVictimRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted)
            .Join(_victimRepository.Entities,
               cV => cV.VictimId,
               v => v.Id,
               (cV, v) => new Dtos.Responses.Victim.VictimResponse
               {
                   Id = v.Id,
                   Name = v.Name,
                   Birthday = v.Birthday,
                   Gender = v.Gender,
                   PhoneNumber = v.PhoneNumber,
                   CitizenId = v.CitizenId,
                   Address = v.Address,
                   Testimony = cV.Testimony,
                   Date = cV.Date,
               }).ToListAsync();
            if (victimOfCase.Any()) response.Victims = victimOfCase;
            var criminalOfCase = await _caseCriminalRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted)
            .Join(_criminalRepository.Entities,
               cCr => cCr.CriminalId,
               c => c.Id,
               (cCr, c) => new Dtos.Responses.Criminal.CriminalResponse
               {
                   Id = c.Id,
                   Code = StaticVariable.CRIMINAL + c.Id.ToString().PadLeft(5, '0'),
                   Name = c.Name,
                   AnotherName = c.AnotherName,
                   Birthday = c.Birthday,
                   Gender = c.Gender,
                   CitizenId = c.CitizenId,
                   HomeTown = c.HomeTown,
                   PermanentResidence = c.PermanentResidence,
                   CurrentAccommodation = c.CurrentAccommodation,
                   Nationality = c.Nationality,
                   Ethnicity = c.Ethnicity,
                   Charge = cCr.Charge,
                   Reason = cCr.Reason,
                   Testimony = cCr.Testimony,
                   Date = cCr.Date,
                   TypeOfViolation = cCr.TypeOfViolation,
                   Weapon = cCr.Weapon,
                   AvatarLink = _uploadService.GetFullUrl(_uploadService.IsFileExists(c.Avatar) ? c.Avatar : "Files/Avatar/NotFound/notFoundAvatar.jpg")
               }).ToListAsync();
            if (criminalOfCase.Any()) response.Criminals = criminalOfCase;
            var imageOfCase = await _caseImageRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
            List<FileResponse> caseImages = new List<FileResponse>();
            foreach (var image in imageOfCase)
            {
                if (image.EvidenceId != null)
                {
                    caseImages.Add(new FileResponse
                    {
                        FileName = image.FileName,
                        FilePath = image.FilePath,
                        FileUrl = _uploadService.GetFullUrl(image.FilePath)
                    });
                }
            }
            response.CaseImages = caseImages;

            var wantedCriminals = await _caseRepository.Entities.Where(_ => _.Id == request.Id && !_.IsDeleted)
           .Join(_wantedCriminalRepository.Entities,
              c => c.Id,
              w => w.CaseId,
              (c, w) => new WantedCriminalOfCaseResponse
              {
                  CriminalId = w.CriminalId,
                  WantedType = w.WantedType,
                  CurrentActivity = w.CurrentActivity,
                  WantedDecisionNo = w.WantedDecisionNo,
                  WantedDecisionDay = w.WantedDecisionDay,
                  DecisionMakingUnit = w.DecisionMakingUnit,
              })
            .ToListAsync();
            foreach (var wantedCriminal in wantedCriminals)
            {
                var caseCriminal = _caseCriminalRepository.Entities.Where(_ => _.CaseId == request.Id && _.CriminalId == wantedCriminal.CriminalId && !_.IsDeleted).FirstOrDefault();
                wantedCriminal.Weapon = caseCriminal == null || String.IsNullOrEmpty(caseCriminal.Weapon) ? "" : caseCriminal.Weapon;
            }
            if (wantedCriminals.Any()) response.WantedCriminalResponse = wantedCriminals;

            return await Result<GetCaseByIdResponse>.SuccessAsync(response);
        }
    }
}
