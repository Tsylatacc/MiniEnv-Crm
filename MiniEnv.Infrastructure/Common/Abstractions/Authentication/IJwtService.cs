using MiniEnv.Infrastructure.DTOs.Common.Authentication;

namespace MiniEnv.Infrastructure.Common.Abstractions.Authentication
{
    public interface IJwtService
    {
        JwtDto GenerateSignUpToken(string signUpTokenHash, Guid tenantId);
        JwtDto GenerateRefreshToken();
        JwtDto GenerateBearerToken(Guid userId, Guid tenantId, string email);

        string GenerateToken(int sizeInBytes);
        string HashToken(string token);

        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
