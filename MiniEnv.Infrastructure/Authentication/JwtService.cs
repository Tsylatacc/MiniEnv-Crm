using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.DTOs.Common.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace MiniEnv.Infrastructure.Authentication
{
    public sealed class JwtService : IJwtService
    {
        private readonly int WorkFactor = 12;
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtDto GenerateBearerToken(Guid userId, Guid tenantId, string email)
        {
            Claim[] claims =
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(CustomClaimTypes.TenantId, tenantId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            SymmetricSecurityKey key = new(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Bearer:Key"]!)
            );

            SigningCredentials credentials = new(
                key,
                SecurityAlgorithms.HmacSha256
            );

            DateTimeOffset expiresAt = DateTimeOffset.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:Bearer:ExpiresInMinutes"]!));

            JwtSecurityToken token = new(
                issuer: _configuration["Jwt:Bearer:Issuer"],
                audience: _configuration["Jwt:Bearer:Audience"],
                claims: claims,
                expires: expiresAt.UtcDateTime,
                signingCredentials: credentials
            );

            return new JwtDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt
            };
        }
        public JwtDto GenerateSignUpToken(Guid signUpId, string email)
        {
            Claim[] claims =
            {
                new Claim(JwtRegisteredClaimNames.Sub, signUpId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            SymmetricSecurityKey key = new(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SignUp:Key"]!)
            );

            SigningCredentials credentials = new(
                key,
                SecurityAlgorithms.HmacSha256
            );

            DateTimeOffset expiresAt = DateTimeOffset.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:SignUp:ExpiresInMinutes"]!));

            JwtSecurityToken token = new(
                issuer: _configuration["Jwt:SignUp:Issuer"],
                audience: _configuration["Jwt:SignUp:Audience"],
                claims: claims,
                expires: expiresAt.UtcDateTime,
                signingCredentials: credentials
            );

            return new JwtDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt
            };
        }

        public JwtDto GenerateRefreshToken()
        {
            return new JwtDto
            {
                Token = GenerateToken(32),
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(int.Parse(_configuration["Jwt:Refresh:ExpiresInDays"]!))
            };
        }

        public string GenerateToken(int sizeInBytes)
        {
            return Convert.ToHexString(
               RandomNumberGenerator.GetBytes(sizeInBytes)
            );
        }

        public string HashToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Argument 'token' cannot be empty.", nameof(token));

            return Convert.ToHexString(
                SHA256.HashData(
                    Encoding.UTF8.GetBytes(token)
                )
            );
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}