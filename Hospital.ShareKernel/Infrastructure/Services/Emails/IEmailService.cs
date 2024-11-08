using Hospital.SharedKernel.Infrastructure.Services.Emails.Models;

namespace Hospital.SharedKernel.Infrastructure.Services.Emails
{
    public interface IEmailService
    {
        Task SendAsync(EmailOptionRequest option, CancellationToken cancellationToken = default);
    }
}
