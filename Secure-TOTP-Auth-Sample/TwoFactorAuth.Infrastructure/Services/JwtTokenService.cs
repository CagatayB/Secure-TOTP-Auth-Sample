using Microsoft.IdentityModel.Tokens;
using Secure_TOTP_Auth_Sample.TwoFactorAuth.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Secure_TOTP_Auth_Sample.TwoFactorAuth.Infrastructure.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(Guid userId, string email)
        {
            
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new("scope", "full_access")
        };

            return CreateToken(claims, int.Parse(_config["JwtSettings:DurationInMinutes"] ?? "60"));
        }

        public string GenerateLimitedToken(Guid userId, string purpose = "2fa")
        {
            // Sadece 2FA endpoint'i için geçerli kısıtlı token
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new("purpose", purpose), // Sadece bu claim'e sahip olanlar 2fa/validate'e gidebilir
            new("scope", "limited_access")
        };

            // Kısıtlı token'ın ömrü çok kısa olmalı (Örn: 5 dakika)
            return CreateToken(claims, 5);
        }
        private string CreateToken(IEnumerable<Claim> claims, int expirationMinutes)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
