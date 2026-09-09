using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MiniEnv.Infrastructure.Authentication;

public sealed class SignupContext : ISignUpContext
{
    private readonly IHttpContextAccessor _context;

    public SignupContext(IHttpContextAccessor context)
    {
        _context = context;
    }

    private ClaimsPrincipal User =>
        _context.HttpContext?.User
        ?? throw new UnauthorizedAccessException("Unauthenticated user.");

    public Guid TenantId => Guid.NewGuid();
    public Guid SignUpId =>
            Guid.Parse(
                User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? throw new UnauthorizedAccessException("SignUpId claim not found."));

    public string Email =>
        User.FindFirst(JwtRegisteredClaimNames.Email)?.Value
        ?? throw new UnauthorizedAccessException("Email claim not found.");
}