using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Data.Config;
using MailKit.Net.Smtp;
using MimeKit;

namespace krov_nad_glavom_api.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;
        public EmailService(EmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(string email, string subject, string textMessage, string htmlMessage)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_emailConfig.SenderName, _emailConfig.SenderEmail));
            mimeMessage.ReplyTo.Add(new MailboxAddress(_emailConfig.SenderName, _emailConfig.SenderEmail));
            mimeMessage.To.Add(MailboxAddress.Parse(email));
            mimeMessage.Subject = subject;

            var builder = new BodyBuilder { TextBody = textMessage, HtmlBody = htmlMessage };

            mimeMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.MailServer, _emailConfig.MailPort, MailKit.Security.SecureSocketOptions.SslOnConnect);

                    await client.AuthenticateAsync(_emailConfig.AdminEmail, _emailConfig.Password);

                    await client.SendAsync(mimeMessage);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to send email.", ex);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }

        public string GetEmailVerificationHtmlBody(string verificationLink)
        {
			return $@"
				<html>
				<body style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding-top: 50px; padding-bottom: 50px'>
                    <div style='max-width:600px; margin:auto; padding:20px; border:1px solid #ddd; border-radius:10px; background-color:#ffffff;'>
                        <h2>Dobrodošli u KrovNadGlavom!</h2>
                        <p>Hvala vam na prijavi. Molimo vas verifikujte vaš profil klikom na dugme ispod:</p>
                        <a href='{verificationLink}' 
                        style='display:inline-block; padding:10px 20px; margin-top:20px; 
                                font-size:16px; background-color:#c7671e; color:#ffffff; 
                                text-decoration:none; border-radius:5px;'>
                            Verifikuj profil
                        </a>
                        <p>Ako niste poslali zahtev za ovo, slobodno ignorišite ovaj mail.</p>
                        <p>– KrovNadGlavom Team</p>
                    </div>
                </body>
				</html>";
        }
    }
}