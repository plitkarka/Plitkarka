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

    public EmailService(IOptions<EmailConfiguration> emailConfiguration)
    {
        _settings = emailConfiguration.Value;
    }

    public async Task SendEmailAsync(string email,string body,string subject)
    {
        try
        {
            var mail = new MimeMessage();

            mail.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
            mail.Sender = new MailboxAddress(_settings.DisplayName, _settings.From);

            mail.To.Add(MailboxAddress.Parse(email));

            var emailBody = new BodyBuilder();
            mail.Subject = subject;
            emailBody.HtmlBody = body;
            mail.Body = emailBody.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, CancellationToken.None);

            await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, CancellationToken.None);
            await smtp.SendAsync(mail, CancellationToken.None);
            await smtp.DisconnectAsync(true, CancellationToken.None);

        }
        catch (EmailServiceException ex)
        {
            throw new EmailServiceException(ex.Message);
        }
    }
}
