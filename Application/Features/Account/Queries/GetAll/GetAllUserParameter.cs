using Application.Parameters;
using System;
using System.Collections.Generic;
namespace Application.Features.Account.Queries.GetAll
{
    public class GetAllUserParameter : RequestParameter
    {
        public int? RoleId { get; set; }
        public int? YearOfBirth { get; set; }
        public string? Area { get; set; }
    }
}
