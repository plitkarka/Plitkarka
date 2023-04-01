namespace Plitkarka.Domain.Services.EmailService;

public interface IEmailService
{
    Task SendEmailAsync(string email, string body, string subject);
}
