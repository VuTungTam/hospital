using Hospital.SharedKernel.Infrastructure.ExternalServices.VNPay.Models;
using Microsoft.AspNetCore.Http;

namespace Hospital.SharedKernel.Infrastructure.ExternalServices.VNPay
{
    public class VNPayService : IVNPayService
    {
        private readonly IHttpClientFactory _factory;

        private readonly VNPayLibrary vnp;

        public VNPayService(IHttpClientFactory factory)
        {
            _factory = factory;
            vnp = new VNPayLibrary();
        }

        public Task<string> GenerateUrl(VNpayModel model, CancellationToken cancellationToken = default)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(VNPayConfig.TimeZoneId);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            vnp.AddRequestData("vnp_Version", VNPayLibrary.VERSION);
            vnp.AddRequestData("vnp_Command", "pay");
            vnp.AddRequestData("vnp_TmnCode", VNPayConfig.VnpTmnCode);
            vnp.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            vnp.AddRequestData("vnp_CurrCode", "VND");
            vnp.AddRequestData("vnp_TxnRef", tick);
            vnp.AddRequestData("vnp_OrderInfo", $"Thanh toan lich kham cho {model.BookingName}");
            vnp.AddRequestData("vnp_OrderType", "billpayment");
            vnp.AddRequestData("vnp_Locale", "vn");
            vnp.AddRequestData("vnp_ReturnUrl", VNPayConfig.VnpReturnUrl);
            vnp.AddRequestData("vnp_IpAddr", "127.0.0.1");
            vnp.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));

            string paymentUrl = vnp.CreateRequestUrl(VNPayConfig.VnpUrl, VNPayConfig.VnpHashSecret);
            return Task.FromResult(paymentUrl);
        }
        public Task<PaymentResponseModel> PaymentExecute(IQueryCollection collections, CancellationToken cancellationToken = default)
        {
            var pay = new VNPayLibrary();
            var response = pay.GetFullResponseData(collections, VNPayConfig.VnpHashSecret);

            return Task.FromResult(response);
        }
    }
}
