using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Plitkarka.Commons.Configuration;
using Plitkarka.Commons.Exceptions;

namespace Plitkarka.Domain.Services.EmailService;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _settings;

    private Random _random = new Random();
    private static readonly int CodeLength = 6;
    public static readonly string VerifiedCode = String.Empty;

    public EmailService(IOptions<EmailConfiguration> emailConfiguration)
    {
        _settings = emailConfiguration.Value;
    }

    public async Task SendEmailAsync(string email, string body, string subject)
    {
        try
        {
            using var mail = new MimeMessage();

            mail.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
            mail.To.Add(MailboxAddress.Parse(email));
            mail.Subject = subject;
            mail.Body = new BodyBuilder()
            {
                HtmlBody = body
            }.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.UserName, _settings.Password);
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            throw new EmailServiceException(ex.Message);
        }
    }

    public string GenerateEmailCode()
    {
        StringBuilder builder = new StringBuilder(CodeLength);

        for(int i = 0; i < CodeLength; i++)
        {
            builder.Append(_random.Next(0, 10));
        }

        return builder.ToString();
    }
}
