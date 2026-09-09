using MiniEnv.Application.Features.Authentication.SignUps;
using MiniEnv.Domain.Entities;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Workers.Handlers
{
    public sealed class ExpireSignUpCommandHandler
    {
        public async Task Handle(
            ExpireSignUpCommand command,
            MiniEnvDbContext db,
            CancellationToken cancellationToken)
        {
            SignUp? signUp = await db.SignUps.SingleOrDefaultAsync(x => x.Id == command.SignUpId, cancellationToken);
            if (signUp is null) return;

            signUp.SetToExpired();
        }
    }
}
