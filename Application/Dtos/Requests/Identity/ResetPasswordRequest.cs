﻿using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Requests.Identity
{
    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = StaticVariable.NOT_MATCH_PASSWORD)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}