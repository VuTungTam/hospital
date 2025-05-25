using Hospital.SharedKernel.Infrastructure.ExternalServices.VNPay.Models;
using Microsoft.AspNetCore.Http;

namespace Hospital.SharedKernel.Infrastructure.ExternalServices.VNPay
{
    public interface IVNPayService
    {
        Task<string> GenerateUrl(VNpayModel model, CancellationToken cancellationToken = default);
        Task<PaymentResponseModel> PaymentExecute(IQueryCollection collections, CancellationToken cancellationToken = default);
    }
}
