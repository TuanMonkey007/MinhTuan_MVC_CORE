using MailKit.Net.Smtp;
using MimeKit;
using MinhTuan.Domain.Helper.EmailSender;


namespace MinhTuan.Service.Core.Services
{

    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfiguration)
        {
            _emailConfig = emailConfiguration;
        }
        public bool SendEmail(Message message)
        {
            try
            {
                var emailMessage = CreateEmailMessage(message,"EmailVerifyTemplate.html");
                Send(emailMessage);
                return true;
            }
            catch
            {
                return false;
            }
           
        }

        private void Send(MimeMessage emailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                client.Send(emailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        private MimeMessage CreateEmailMessage(Message message, string nameTemplate)
        {
           
            var emailMessage = new MimeMessage();
            
            emailMessage.From.Add(new MailboxAddress("VStyle - Thời trang Việt", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            // Load the template
               
            string relativePath = Path.Combine(Directory.GetCurrentDirectory(), "Helper", "EmailTemplates", nameTemplate);
            string emailTemplate = File.ReadAllText(relativePath);

            // Replace the placeholder with the actual confirmation link
            string emailContent = emailTemplate.Replace("{{CONFIRMATION_LINK}}", message.Content);

            // Set the email body with HTML content
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailContent
            };

            return emailMessage;
            
        }

        public bool SendForgotPasswordEmail(Message message)
        {
            try
            {
                var emailMessage = CreateEmailMessage(message, "EmailForgotPasswordTemplate.html");
                Send(emailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
