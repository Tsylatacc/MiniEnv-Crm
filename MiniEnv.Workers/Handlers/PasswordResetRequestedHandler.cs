using MiniEnv.Application.Features.Authentication.ForgotPassword;
using MiniEnv.Infrastructure.Common.Abstractions.Communication;
using MiniEnv.Infrastructure.Persistence;
using MiniEnv.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wolverine;

namespace MiniEnv.Workers.Handlers
{
    public sealed class PasswordResetRequestedHandler
    {

        public async Task Handle(
            PasswordResetRequested message,
            IEmailService emailService,
            MiniEnvDbContext db,
            IMessageBus bus,
            IOptions<FrontEndSettings> frontEnd,
            CancellationToken cancellationToken)
        {
            if (!await db.PasswordResetTokens
                .AnyAsync(x => x.Id == message.PasswordResetTokenId, cancellationToken)) return;

            string link = $"{frontEnd.Value.ActiveUrl}/auth/password/reset/{message.ForgotPasswordToken}";
            await emailService.SendEmailAsync(
                message.Email,
                "Redefinição de senha — MiniEnv",
                $"""
                Olá! Recebemos uma solicitação para redefinir sua senha do MiniEnv.
                <br> <a href='{link}'>Clique aqui para redefinir sua senha</a>.
                <br> Se você não solicitou a redefinição de senha, por gentileza, ignore este e-mail.
                """);
        }
    }
}
