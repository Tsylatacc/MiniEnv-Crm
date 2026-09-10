using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniEnv.Application.Features.Authentication.SignUpTokens;
using MiniEnv.Application.Features.Authentication.VerifySignUps;
using MiniEnv.Domain.Entities;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.DTOs.Common.Authentication;
using MiniEnv.Infrastructure.Persistence;

namespace MiniEnv.Application.Features.Authentication.SignUps
{
    public sealed class SignUpHandler
    {
        public async Task<SignUpResult> Handle(
            SignUpCommand command,
            MiniEnvDbContext db,
            IJwtService jwtService,
            ILogger<SignUpHandler> logger,
            CancellationToken cancellationToken)
        {
            SignUpToken? signUp = await db.SignUpTokens.SingleOrDefaultAsync(x => x.Email == command.Email, cancellationToken);
            if (signUp is not null && signUp.UsedAt is not null)
                throw new InvalidOperationException($"Invalid sign-up {signUp.Id} status.");

            string signUpToken = jwtService.GenerateToken(32);
            string signUpTokenHash = jwtService.HashToken(signUpToken);

            SignUpToken newSignUp = SignUpToken.Create(command.Email, signUpTokenHash, DateTimeOffset.UtcNow.AddMinutes(15));
            await db.SignUpTokens.AddAsync(newSignUp, cancellationToken);

            JwtDto dto = jwtService.GenerateSignUpToken(signUpTokenHash, Guid.NewGuid());
            logger.LogInformation("Sign-up verified for {SignUpId}", newSignUp.Id);
            return new SignUpResult(dto.Token);
        }
    }
}
