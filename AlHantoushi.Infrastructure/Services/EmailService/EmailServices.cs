using AlHantoushi.Infrastructure.Services.EmailService.Interfaces;
using MailKit.Net.Smtp;

using MimeKit;

namespace AlHantoushi.Infrastructure.Services.EmailService
{
    public class EmailServices : IEmailServices
    {
        public EmailConfiguration _emailConfig { get; }

        public EmailServices(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public async Task SendSingleEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendEmailAsync(emailMessage); 
        }

        public async Task SendBulkEmailAsync(Message message, int batchSize = 50)
        {
            var totalRecipients = message.To.Count;
            var batches = (int)Math.Ceiling((double)totalRecipients / batchSize);

            for (int i = 0; i < batches; i++)
            {
                var batchRecipients = message.To.Skip(i * batchSize).Take(batchSize).ToList();

                var emailMessage = CreateEmailMessageForBatch(message, batchRecipients);

                await SendEmailAsync(emailMessage);
            }
        }
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Ping Information Technology", _emailConfig.from));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<p>{message.Content}</p>"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
        private MimeMessage CreateEmailMessageForBatch(Message originalMessage, List<MailboxAddress> batchRecipients)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Ping Information Technology", _emailConfig.from));

            emailMessage.To.AddRange(batchRecipients);

            emailMessage.Subject = originalMessage.Subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<p>{originalMessage.Content}</p>"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private async Task SendEmailAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.SmtpUsername, _emailConfig.SmtpPassword);
                await client.SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
