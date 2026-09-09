using MiniEnv.Application.Features.Authentication.SignUps;
using MiniEnv.Application.Features.Users.Invite;
using MiniEnv.Infrastructure.Common.Abstractions.Communication;
using MiniEnv.Infrastructure.Persistence;
using MiniEnv.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wolverine;

namespace MiniEnv.Workers.Handlers
{
    public sealed class UserInvitationRequestedHandler
    {

        public async Task Handle(
            InvitationRequested message,
            IEmailService emailService,
            MiniEnvDbContext db,
            IMessageBus bus,
            IOptions<FrontEndSettings> frontEnd,
            CancellationToken cancellationToken)
        {
            if (!await db.Invitations
                .AnyAsync(x => x.Id == message.InvitationId, cancellationToken)) return;

            string link = $"{frontEnd.Value.ActiveUrl}/api/users/invitations/accept/{message.InvitationToken}";
            await emailService.SendEmailAsync(
                message.Email,
                "Você recebeu um convite para o MiniEnv",
                $"Olá! Você recebeu um convite para fazer parte do MiniEnv." +
                $"<br> <a href='{link}'>Clique aqui para aceitar seu convite e realizar seu cadastro no MiniENv</a>." +
                $"<br> Se você não esperava receber este convite, por gentileza, ignore este e-mail.");

            await bus.ScheduleAsync(new ExpireSignUpCommand(message.InvitationId), TimeSpan.FromMinutes(15));
        }
    }
}
