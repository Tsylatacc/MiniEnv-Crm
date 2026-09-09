using MiniEnv.Infrastructure.Common.Abstractions.Communication;
using MiniEnv.Infrastructure.Settings;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MiniEnv.Infrastructure.Communication
{
    public sealed class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_settings.FromName, _settings.User));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            message.Body = new TextPart(isHtml ? "html" : "plain")
            {
                Text = body
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.User, _settings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}