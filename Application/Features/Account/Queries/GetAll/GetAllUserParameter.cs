using Application.Parameters;
using Domain.Constants.Enum;

namespace Application.Features.Account.Queries.GetAll
{
    public class GetAllUserParameter : RequestParameter
    {
        public Role? RoleId { get; set; }
        public int? YearOfBirth { get; set; }
        public string? Area { get; set; }
    }
}
