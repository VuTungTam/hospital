using Microsoft.Extensions.Configuration;

namespace Hospital.SharedKernel.Infrastructure.ExternalServices.VNPay.Models
{
    public class VNPayConfig
    {
        public static string VnpUrl { get; private set; }

        public static string VnpApi { get; private set; }

        public static string VnpTmnCode { get; private set; }

        public static string VnpHashSecret { get; private set; }

        public static string VnpReturnUrl { get; private set; }

        public static string TimeZoneId { get; private set; }

        public static void Set(IConfiguration configuration)
        {
            VnpUrl = configuration.GetRequiredSection("External:VNPay:VNPUrl").Value;
            VnpApi = configuration.GetRequiredSection("External:VNPay:VNPApi").Value;
            VnpTmnCode = configuration.GetRequiredSection("External:VNPay:VNPTmnCode").Value;
            VnpHashSecret = configuration.GetRequiredSection("External:VNPay:VNPHashSecret").Value;
            VnpReturnUrl = configuration.GetRequiredSection("External:VNPay:VnpReturnUrl").Value;
            TimeZoneId = configuration.GetRequiredSection("TimeZoneId").Value;
        }
    }
}
