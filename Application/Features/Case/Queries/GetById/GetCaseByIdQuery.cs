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
            var caseCheck = await _caseRepository.FindAsync(_ => _.Id == request.Id && !_.IsDeleted);
            if (caseCheck == null)
            {
                return await Result<GetCaseByIdResponse>.FailAsync(StaticVariable.NOT_FOUND_MSG);
            }
            var response = _mapper.Map<GetCaseByIdResponse>(caseCheck);
            var evidenceOfCase = await _evidenceRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
            if (evidenceOfCase.Any()) response.Evidences = _mapper.Map<List<Dtos.Responses.Evidence.EvidenceResponse>>(evidenceOfCase).ToList();
            var witnessOfCase = await _caseWitnessRepository.Entities.Where(_ => _.CaseId == request.Id)
            .Join(_witnessRepository.Entities,
                cW => cW.WitnessId,
                w => w.Id,
                (cw, c) => new Dtos.Responses.Witness.WitnessResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    CitizenId = c.CitizenId,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    Date = c.Date,
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
                    Birthday = i.Birthday.Value.ToString("dd/MM/yyyy"),
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
                   Birthday = v.Birthday.Value.ToString("dd/MM/yyyy"),
                   Gender = v.Gender,
                   PhoneNumber = v.PhoneNumber,
                   CitizenId = v.CitizenId,
                   Address = v.Address,
                   Testimony = cV.Testimony
               }).ToListAsync();
            if (victimOfCase.Any()) response.Victims = victimOfCase;
            var criminalOfCase = await _caseCriminalRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted)
            .Join(_criminalRepository.Entities,
               cCr => cCr.CriminalId,
               c => c.Id,
               (cCr, c) => new Dtos.Responses.Criminal.CriminalResponse
               {
                   Id = c.Id,
                   Name = c.Name,
                   Birthday = c.Birthday.ToString("dd/MM/yyyy"),
                   Gender = c.Gender,
                   PhoneNumber = c.PhoneNumber,
                   Address = c.CurrentAccommodation,
                   Testimony = cCr.Testimony
               }).ToListAsync();
            if (criminalOfCase.Any()) response.Criminals = criminalOfCase;
            var imageOfCase = await _caseImageRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
            response.CaseImages = imageOfCase.Select(i => new FileResponse
            {
                FileName = i.FileName,
                FilePath = i.FilePath,
                FileUrl = _uploadService.GetFullUrl(i.FilePath)
            }).ToList();

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
              }).ToListAsync();
            if (wantedCriminals.Any()) response.WantedCriminalResponse = wantedCriminals;

            return await Result<GetCaseByIdResponse>.SuccessAsync(response);
        }
    }
}
