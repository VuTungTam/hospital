using Hospital.SharedKernel.Infrastructure.Services.Sms.Models;

namespace Hospital.SharedKernel.Infrastructure.Services.Sms
{
    public interface ISmsService
    {
        Task SendAsync(SmsRequest request, bool checkQuota, CancellationToken cancellationToken = default);
    }
}
