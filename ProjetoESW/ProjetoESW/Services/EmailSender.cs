using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ESW19Backup2.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task Execute (string apiKey, string subject, string message, string email)
        {
            /* var client = new SendGridClient(apiKey);
             var msg = new SendGridMessage()
             {
                 From = new MailAddress("AlberCool@pt.pt", "AlberCool"),
                 Subject = subject,
                 PlainTextContent = message,
                 HtmlContent = message
             };

             msg.AddTo(new MailAddress(email));
             return client.SendEmailAsync(msg);
             */
            return null;
        }
    }
}
