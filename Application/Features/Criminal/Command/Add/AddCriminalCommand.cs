using Application.Dtos.Requests.Image;
using Application.Interfaces.Criminal;
using Application.Interfaces.CriminalImage;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Entities.CriminalImage;
using Domain.Wrappers;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Features.Criminal.Command.Add
{
    public class AddCriminalCommand : IRequest<Result<AddCriminalCommand>>
    {
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_NAME)]
        public string Name { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_ANOTHER_NAME)]
        public string AnotherName { get; set; } = null!;
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_CITIZEN_ID)]
        public string CitizenID { get; set; } = null!;
        public bool? Gender { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly Birthday { get; set; }
        [RegularExpression(@"(\+84|84|0)+(3|5|7|8|9|1[2|6|8|9])+([0-9]{8,10})\b", ErrorMessage = StaticVariable.INVALID_PHONE_NUMBER)]
        [MaxLength(15, ErrorMessage = StaticVariable.LIMIT_PHONENUMBER)]
        [DefaultValue("string")]
        public string PhoneNumber { get; set; } = null!;
        [MaxLength(100,ErrorMessage = StaticVariable.LIMIT_PHONE_MODEL)]
        public string PhoneModel { get; set; } = null!;
        [MaxLength(300, ErrorMessage = StaticVariable.LIMIT_CAREER_AND_WORKPLACE)]
        public string CareerAndWorkplace { get; set; } = null!;
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_CHRACTERISTICS)]
        public string Characteristics { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_HOME_TOWN)]
        public string HomeTown { get; set; } = null!;
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_ETHNICITY)]
        public string Ethnicity { get; set; } = null!;
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_RELIGION)]
        public string? Religion { get; set; }
        [MaxLength(50, ErrorMessage = StaticVariable.LIMIT_NATIONALITY)]
        public string Nationality { get; set; } = null!;
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_FATHER_NAME)]
        public string FatherName { get; set; } = null!;
        [MaxLength(12, ErrorMessage = StaticVariable.LIMIT_FATHER_CITIZEN_ID)]
        public string FatherCitizenID { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly FatherBirthday { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_MOTHER_NAME)]
        public string MotherName { get; set; } = null!;
        [MaxLength(12, ErrorMessage = StaticVariable.LIMIT_MOTHER_CITIZEN_ID)]
        public string MotherCitizenID { get; set; } = null!;
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly MotherBirthday { get; set; }
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_PERMANENT_RESIDENCE)]
        public string PermanentResidence { get; set; } = null!;
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_CURRENT_ACCOMMODATION)]
        public string CurrentAccommodation { get; set; } = null!;
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_ENTRY_AND_EXITINFORMATION)]
        public string? EntryAndExitInformation { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_FACEBOOK)]
        public string? Facebook { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_ZALO)]
        public string? Zalo { get; set; }
        [MaxLength(300, ErrorMessage = StaticVariable.LIMIT_OTHER_SOCIAL_NETWORKS)]
        public string? OtherSocialNetworks { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_GAME_ACCOUNT)]
        public string? GameAccount { get; set; }
        [MaxLength(30, ErrorMessage = StaticVariable.LIMIT_BANK_ACCOUNT)]
        public string? BankAccount { get; set; }
        public string? Research { get; set; }
        [MaxLength(100, ErrorMessage = StaticVariable.LIMIT_VEHICLES)]
        public string? Vehicles { get; set; }
        [MaxLength(200, ErrorMessage = StaticVariable.LIMIT_DANGEROUS_LEVEL)]
        public string? DangerousLevel { get; set; }
        public string? ApproachArrange { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly DateOfMostRecentCrime { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? ReleaseDate { get; set; }
        public CriminalStatus Status { get; set; }
        [MaxLength(500, ErrorMessage = StaticVariable.LIMIT_OTHER_INFORMATION)]
        public string? OtherInformation { get; set; }
        public List<ImageRequest>? CriminalImages { get; set; }

    }
    internal class AddCriminalCommandHandler : IRequestHandler<AddCriminalCommand, Result<AddCriminalCommand>>
    {
        private readonly ICriminalRepository _criminalRepository; 
        private readonly IUnitOfWork<long> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICriminalImageRepository _criminalImageRepository;

        public AddCriminalCommandHandler(ICriminalRepository criminalRepository, IUnitOfWork<long> unitOfWork,
            IMapper mapper, ICriminalImageRepository criminalImageRepository)
        {
            _criminalRepository = criminalRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _criminalImageRepository = criminalImageRepository;
        }

        public async Task<Result<AddCriminalCommand>> Handle(AddCriminalCommand request, CancellationToken cancellationToken)
        {
            var isCitizenIDExists = await _criminalRepository.FindAsync(_ => _.CitizenID.Equals(request.CitizenID));
            if (isCitizenIDExists != null)
            {
                return await Result<AddCriminalCommand>.FailAsync(StaticVariable.CITIZEN_ID_EXISTS_MSG);
            }
            var addCriminal = _mapper.Map<Domain.Entities.Criminal.Criminal>(request);
            await _criminalRepository.AddAsync(addCriminal);
            await _unitOfWork.Commit(cancellationToken);
            if (request.CriminalImages != null)
            {
                var addImage = _mapper.Map<List<CriminalImage>>(request.CriminalImages);
                addImage.ForEach(x => x.CriminalId = addCriminal.Id);
                await _criminalImageRepository.AddRangeAsync(addImage);
            }
            return await Result<AddCriminalCommand>.SuccessAsync(request);
        }
    }
}
