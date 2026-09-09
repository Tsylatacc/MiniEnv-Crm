using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace MiniEnv.Application.Features.Authentication.SignUps
{
    public sealed class SignUpHandler
    {
        public async Task Handle(
            SignUpCommand command,
            MiniEnvDbContext db,
            IJwtService jwtService,
            IMessageBus bus,
            ILogger<SignUpHandler> logger,
            CancellationToken cancellationToken)
        {
            SignUp? signUp = await db.SignUps.SingleOrDefaultAsync(x => x.Email == command.Email, cancellationToken);
            if (signUp is not null && signUp.Status != SignUpStatus.Expired) return;

            string signUpToken = jwtService.GenerateToken(4);
            string signUpTokenHash = jwtService.HashToken(signUpToken);

            SignUp newSignUp = SignUp.Create(command.Email, signUpTokenHash, SignUpStatus.Pending);
            await db.SignUps.AddAsync(newSignUp, cancellationToken);
            await bus.PublishAsync(new SignUpRequested(newSignUp.Id, newSignUp.Email, signUpToken));
            logger.LogInformation("Sign-up token issued for sign-up {SignUpId}", newSignUp.Id);
        }
    }
}
