﻿using Domain.Constants.Enum;
using System.Text.Json.Serialization;

namespace Application.Features.Account.Queries.GetById
{
    public class GetAccountByIdResponse
    {
        public string Name { get; set; } = null!;
        [JsonPropertyName("citizen_id")]
        public string CitizenID { get; set; } = null!;
        public bool? Gender { get; set; }
        [JsonConverter(typeof(CustomConverter.DateOnlyConverter))]
        public DateOnly? Birthday { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
        public string AccountName { get; set; } = null!;
        public Role Role { get; set; }
        public string? Image { get; set; }
        public string? ImageLink { get; set; }
    }
}
