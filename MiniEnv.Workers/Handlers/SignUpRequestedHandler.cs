using MiniEnv.Application.Features.Authentication.SignUps;
using MiniEnv.Infrastructure.Common.Abstractions.Communication;
using MiniEnv.Infrastructure.Persistence;
using MiniEnv.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wolverine;

namespace MiniEnv.Workers.Handlers
{
    public sealed class SignUpRequestedHandler
    {

        public async Task Handle(
            SignUpRequested message,
            IEmailService emailService,
            MiniEnvDbContext db,
            IMessageBus bus,
            IOptions<FrontEndSettings> frontEnd,
            CancellationToken cancellationToken)
        {
            if (!await db.SignUps
                .AnyAsync(x => x.Id == message.SignUpId, cancellationToken)) return;

            string link = $"{frontEnd.Value.ActiveUrl}/auth/signup/verify/{message.SignUpToken}";
            await emailService.SendEmailAsync(
                message.Email,
                "Seu cadastro no MiniEnv foi concluído",
                $"Olá! Seu acesso ao MiniEnv foi criado com sucesso. " +
                $"<br> <a href='{link}'>Clique aqui para acessar o MiniENv</a>." +
                $"<br> Se você não solicitou este cadastro, por gentileza, ignore este e-mail.");

            await bus.ScheduleAsync(new ExpireSignUpCommand(message.SignUpId), TimeSpan.FromMinutes(15));
        }
    }
}
