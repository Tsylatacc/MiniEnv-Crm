using Microsoft.Extensions.Options;
using MiniEnv.Infrastructure.Common.Abstractions.Communication;
using MiniEnv.Infrastructure.Settings;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MiniEnv.Infrastructure.Communication;

public class EmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly string _senderEmail;
    private readonly string _senderName;

    public EmailService(HttpClient httpClient, IOptions<MailerSendOptions> options)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.mailersend.com/v1/");
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", options.Value.ApiKey);

        _senderEmail = options.Value.SenderEmail;
        _senderName = options.Value.SenderName;
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml)
    {
        var payload = new MailerSendRequest
        {
            From = new MailerSendContact { Email = _senderEmail, Name = _senderName },
            To = new List<MailerSendContact> { new() { Email = to } },
            Subject = subject,
            Html = isHtml ? body : null,
            Text = isHtml ? null : body
        };

        var response = await _httpClient.PostAsJsonAsync("email", payload);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"MailerSend failed ({response.StatusCode}): {errorBody}");
        }
    }
}

file class MailerSendRequest
{
    [JsonPropertyName("from")] public MailerSendContact From { get; set; } = default!;
    [JsonPropertyName("to")] public List<MailerSendContact> To { get; set; } = default!;
    [JsonPropertyName("subject")] public string Subject { get; set; } = default!;
    [JsonPropertyName("html")] public string? Html { get; set; }
    [JsonPropertyName("text")] public string? Text { get; set; }
}

file class MailerSendContact
{
    [JsonPropertyName("email")] public string Email { get; set; } = default!;
    [JsonPropertyName("name")] public string? Name { get; set; }
}