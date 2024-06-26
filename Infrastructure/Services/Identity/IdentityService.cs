﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Dtos.Requests.Identity;
using Application.Dtos.Responses.Identity;
using Application.Interfaces;
using Application.Interfaces.Services.Identity;
using Domain.Constants;
using Domain.Constants.Enum;
using Domain.Entities;
using Domain.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations;

namespace Infrastructure.Services.Identity
{
    public class IdentityService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppConfiguration _appConfig;
        private readonly IUploadService _uploadService;

        private DateTime tokenExpireTime;

        public IdentityService(UserManager<AppUser> userManager, IOptions<AppConfiguration> appConfig, IUploadService uploadService)
        {
            _userManager = userManager;
            _appConfig = appConfig.Value;
            _uploadService = uploadService;
        }

        public async Task<Result<TokenResponse>> LoginAsync(TokenRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return await Result<TokenResponse>.FailAsync(StaticVariable.ACCOUNT_IS_NOT_CORRECT);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return await Result<TokenResponse>.FailAsync(StaticVariable.ACCOUNT_IS_NOT_CORRECT);
            }

            if(user.IsDeleted)
                return await Result<TokenResponse>.FailAsync(StaticVariable.ACCOUNT_IS_NOT_EXIST);

            if (!user.IsActive)
                return await Result<TokenResponse>.FailAsync(StaticVariable.ACCOUNT_IS_NOT_ACTIVE);

            var token = await GenerateJwtAsync(user);

            user.RefreshToken = GenerateRefreshToken();
            user.TokenExpiryTime = tokenExpireTime;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            Role role = Role.None;
            if (roles != null && roles.Count > 0)
                role = (Role)Enum.Parse(typeof(Role), roles.First());

            var response = new TokenResponse
            {
                Token = token,
                RefreshToken = user.RefreshToken,
                AvatarUrl = _uploadService.GetFullUrl(user.AvatarUrl),
                Email = user.Email,
                Username = user.UserName,
                Role = role,
                TokenExpiryTime = user.TokenExpiryTime,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                UserId = user.UserId
            };
            return await Result<TokenResponse>.SuccessAsync(response);
        }

        public async Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model)
        {
            if (model is null)
                return await Result<TokenResponse>.FailAsync(StaticVariable.INVALID_TOKEN);

            var userPrincipal = GetPrincipalFromExpiredToken(model.Token);
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            if(userEmail == null)
                return await Result<TokenResponse>.FailAsync(StaticVariable.INVALID_TOKEN);

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null || user.IsDeleted)
                return await Result<TokenResponse>.FailAsync(StaticVariable.ACCOUNT_IS_NOT_EXIST);

            if (!user.IsActive)
                return await Result<TokenResponse>.FailAsync(StaticVariable.ACCOUNT_IS_NOT_ACTIVE);

            if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return await Result<TokenResponse>.FailAsync(StaticVariable.INVALID_REFRESH_TOKEN);

            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            user.RefreshToken = GenerateRefreshToken();
            user.TokenExpiryTime = tokenExpireTime;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            Role role = Role.None;
            if (roles != null && roles.Count > 0)
                role = (Role)Enum.Parse(typeof(Role), roles.First());

            var response = new TokenResponse
            {
                Token = token,
                RefreshToken = user.RefreshToken,
                AvatarUrl = _uploadService.GetFullUrl(user.AvatarUrl),
                Email = user.Email,
                Username = user.UserName,
                Role = role,
                TokenExpiryTime = user.TokenExpiryTime,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                UserId = user.UserId
            };
            return await Result<TokenResponse>.SuccessAsync(response);
        }

        #region Private function

        private async Task<string> GenerateJwtAsync(AppUser user)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            return token;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

            var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.UserName),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Name, user.FullName),
                    new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
                }
                .Union(userClaims)
                .Union(roleClaims);

            return claims;
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            tokenExpireTime = DateTime.UtcNow.AddHours(2);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: tokenExpireTime,
                signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException(StaticVariable.INVALID_TOKEN);
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_appConfig.Secret);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        #endregion Private function
    }
}