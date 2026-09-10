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

    public string SignUpTokenHash =>
        User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
        ?? throw new UnauthorizedAccessException("SignUpTokenHash claim not found.");

    public Guid TenantId =>
        Guid.Parse(User.FindFirst(CustomClaimTypes.TenantId)?.Value
        ?? throw new UnauthorizedAccessException("TenantId claim not found."));
}