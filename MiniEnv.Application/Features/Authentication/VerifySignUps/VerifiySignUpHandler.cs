using MiniEnv.Domain.Entities;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.DTOs.Common.Authentication;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MiniEnv.Application.Features.Authentication.VerifySignUps
{
    public sealed class VerifySignUpHandler
    {
        public async Task<VerifySignUpResponse> Handle(
            VerifySignUpCommand command,
            MiniEnvDbContext db,
            IJwtService jwtService,
            ILogger<VerifySignUpHandler> logger,
            CancellationToken cancellationToken)
        {
            string hashedSignUpToken = jwtService.HashToken(command.SignUpToken);
            SignUp signUp = await db.SignUps
                .Where(x => x.TokenHash == hashedSignUpToken)
                .SingleOrDefaultAsync(cancellationToken) ??
                    throw new KeyNotFoundException($"Sign-up request {command.SignUpToken} not found.");

            signUp.SetToVerified();
            JwtDto dto = jwtService.GenerateSignUpToken(signUp.Id, signUp.Email);
            logger.LogInformation("Sign-up verified for {SignUpId}", signUp.Id);
            return new VerifySignUpResponse(dto.Token, dto.ExpiresAt);
        }
    }
}
