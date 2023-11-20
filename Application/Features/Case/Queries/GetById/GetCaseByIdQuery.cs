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
        private readonly IMapper _mapper;

        public GetCaseByIdQueryHandler(ICaseRepository caseRepository, ICaseCriminalRepository caseCriminalRepository,
            ICriminalRepository criminalRepository, IEvidenceRepository evidenceRepository,
            ICaseWitnessRepository caseWitnessRepository, IWitnessRepository witnessRepository,
            ICaseImageRepository caseImageRepository, IUploadService uploadService,
            ICaseInvestigatorRepository caseInvestigatorRepository, IAccountRepository accountRepository,
            ICaseVictimRepository caseVictimRepository, IVictimRepository victimRepository,
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
            if (evidenceOfCase.Any()) response.Evidences?.AddRange(_mapper.Map<List<Dtos.Responses.Evidence.EvidenceResponse>>(evidenceOfCase));
            var witnessOfCase = await _caseWitnessRepository.Entities.Where(_ => _.CaseId == request.Id)
            .Join(_witnessRepository.Entities,
                cW => cW.WitnessId,
                w => w.Id,
                (cw, c) => new Dtos.Responses.Witness.WitnessResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    CitizenID = c.CitizenID,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    Testimony = c.Testimony,
                    Date = c.Date
                }).ToListAsync();
            if (witnessOfCase.Any()) response.Witnesses?.AddRange(witnessOfCase);
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
            if (investigatorOFCase.Any()) response.Investigators?.AddRange(investigatorOFCase);
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
                   CitizenID = v.CitizenID,
                   Address = v.Address
               }).ToListAsync();
            if (victimOfCase.Any()) response.Victims?.AddRange(victimOfCase);
            var criminalOfCase = await _caseCriminalRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted)
            .Join(_criminalRepository.Entities,
               cCr => cCr.CriminalId,
               c => c.Id,
               (cCr, c) => new Dtos.Responses.Criminal.CriminalResponse
               {
                   Id = c.Id,
                   Name = c.Name,
                   Birthday = c.Birthday,
                   Gender = c.Gender,
                   PhoneNumber = c.PhoneNumber,
                   Address = c.CurrentAccommodation
               }).ToListAsync();
            if (criminalOfCase.Any()) response.Criminals?.AddRange(criminalOfCase);
            var imageOfCase = await _caseImageRepository.Entities.Where(_ => _.CaseId == request.Id && !_.IsDeleted).ToListAsync();
            if (imageOfCase.Any())
            {
                foreach (var image in imageOfCase)
                {
                    response.CaseImages?.Add(_uploadService.GetFullUrl(image.FilePath));
                }
            }
            return await Result<GetCaseByIdResponse>.SuccessAsync(response);
        }
    }
}
