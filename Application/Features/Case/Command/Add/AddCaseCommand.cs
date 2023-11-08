using Application.Dtos.Requests.Image;
using Application.Features.Criminal.Command.Add;
using Application.Interfaces.Case;
using Application.Interfaces.CaseCriminal;
using Application.Interfaces.CaseImage;
using Application.Interfaces.Criminal;
using Application.Interfaces.Evidence;
using Application.Interfaces.Repositories;
using Application.Interfaces.Witness;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Wrappers;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Case.Command.Add
{
    public class AddCaseCommand : IRequest<Result<AddCaseCommand>>
    {
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_REASON)]
        public string? Reason { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_MURDER_WEAPON)]
        public string? MurderWeapon { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TypeOfViolation TypeOfViolation { get; set; }
        public short Status { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_CHARGE)]
        public string Charge { get; set; } = null!;
        public List<Evidence>? Evidences { get; set; }
        public class Evidence
        {
            [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
            public string Name { get; set; } = null!;
            [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_DESCRIPTION)]
            public string? Description { get; set; }
        }
        public List<Witness> Witnesses { get; set; }
        public class Witness
        {
            [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
            public string Name { get; set; } = null!;
            [MaxLength(12, ErrorMessage = StaticVariable.LIMIT_CMND_CCCD)]
            public string CMND_CCCD { get; set; } = null!;
            [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_PHONENUMBER)]
            [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*()-_=+[\]{}|;:',.<>\/?~]{8,}$", ErrorMessage = StaticVariable.INVALID_PHONE_NUMBER)]
            [DefaultValue("stringst")]
            public string PhoneNumber { get; set; } = null!;
            [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_ADDRESS)]
            public string Address { get; set; } = null!;
            public string Testimony { get; set; } = null!;
            public DateTime Date { get; set; }
        }
        public List<ImageRequest>? CaseImage { get; set; }
        public List<long>? CriminalIds { get; set; }

    }
    internal class AddCaseCommandHandler : IRequestHandler<AddCaseCommand, Result<AddCaseCommand>>
    {
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseImageRepository _caseImageRepository;
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        private readonly IEvidenceRepository _evidenceRepository;
        private readonly IWitnessRepository _witnessRepository;
        private readonly ICriminalRepository _criminalRepository;
        private readonly IMapper _mapper;
        public AddCaseCommandHandler(IUnitOfWork<long> unitOfWork, ICaseRepository caseRepository,
            ICaseImageRepository caseImageRepository, ICaseCriminalRepository caseCriminalRepository,
            IEvidenceRepository evidenceRepository, IWitnessRepository witnessRepository,
            ICriminalRepository criminalRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _caseRepository = caseRepository;
            _caseImageRepository = caseImageRepository;
            _caseCriminalRepository = caseCriminalRepository;
            _evidenceRepository = evidenceRepository;
            _witnessRepository = witnessRepository;
            _criminalRepository = criminalRepository;
            _mapper = mapper;
        }
        public async Task<Result<AddCaseCommand>> Handle(AddCaseCommand request, CancellationToken cancellationToken)
        {
            var addCase = _mapper.Map<Domain.Entities.Case.Case>(request);
            await _caseRepository.AddAsync(addCase);
            await _unitOfWork.Commit(cancellationToken);
            if (request.CriminalIds != null)
            {
                foreach (var criminalId in request.CriminalIds)
                {
                    var isCriminalExists = await _criminalRepository.FindAsync(_ => _.Id == criminalId);
                    if (isCriminalExists == null)
                    {
                        return await Result<AddCaseCommand>.FailAsync(StaticVariable.NOT_FOUND_CRIMINAL);
                    }
                    await _caseCriminalRepository.AddAsync(new Domain.Entities.CaseCriminal.CaseCriminal
                    {
                        CaseId = addCase.Id,
                        CriminalId = criminalId
                    });
                }
            }
            if (request.CaseImage != null)
            {
                var addImage = _mapper.Map<List<Domain.Entities.CaseImage.CaseImage>>(request.CaseImage);
                addImage.ForEach(x => x.CaseId = addCase.Id);
                await _caseImageRepository.AddRangeAsync(addImage);
            }
            await _unitOfWork.Commit(cancellationToken);
            if (request.Witnesses != null)
            {
                var addWitnesses = _mapper.Map<List<Domain.Entities.Witness.Witness>>(request.Witnesses);
                addWitnesses.ForEach(x => x.CaseId = addCase.Id);
                await _witnessRepository.AddRangeAsync(addWitnesses);
            }
            if (request.Evidences != null)
            {
                var addEvidences = _mapper.Map<List<Domain.Entities.Evidence.Evidence>>(request.Evidences);
                addEvidences.ForEach(x => x.CaseId = addCase.Id);
                await _evidenceRepository.AddRangeAsync(addEvidences);
            }
            return await Result<AddCaseCommand>.SuccessAsync(request);
        }
    }
}
