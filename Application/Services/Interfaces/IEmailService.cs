namespace krov_nad_glavom_api.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string textMessage, string htmlMessage);
        string GetEmailVerificationHtmlBody(string verificationLink);
    }
}