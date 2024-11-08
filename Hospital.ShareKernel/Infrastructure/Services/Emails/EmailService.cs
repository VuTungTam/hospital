using Hospital.SharedKernel.Infrastructure.Services.Emails.Models;
using Serilog;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Hospital.SharedKernel.Infrastructure.Services.Emails
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(EmailOptionRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var message = new MailMessage(request.Sender, request.To)
                {
                    From = new MailAddress(request.Sender, request.DisplayName),
                    Subject = request.Subject,
                    SubjectEncoding = Encoding.UTF8,
                    Body = request.Body,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = request.IsBodyHTML,
                    Priority = request.Priority
                };
                message.ReplyToList.Add(new MailAddress(request.Sender));

                await SmtpClient.SendMailAsync(message);
                Log.Logger.Information("Sent mail successfully. {Email} - {Subject} - {Body}", request.To, request.Subject, request.Body);
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Send failed. {Exception} - {Email} - {Subject} - {Body}", ex, request.To, request.Subject, request.Body);
                throw;
            }
        }

        private static SmtpClient SmtpClient => new()
        {
            Host = EmailConfig.Host,
            Port = EmailConfig.Port,
            UseDefaultCredentials = false,
            EnableSsl = true,
            Credentials = new NetworkCredential(EmailConfig.Sender, EmailConfig.AppPassword),
        };
    }
}
