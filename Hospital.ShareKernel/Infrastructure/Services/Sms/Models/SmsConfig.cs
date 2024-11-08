using Microsoft.Extensions.Configuration;

namespace Hospital.SharedKernel.Infrastructure.Services.Sms.Models
{
    public class SmsConfig
    {
        public static string BaseUrl { get; private set; }
        public static string ApiKey { get; private set; }
        public static string ApiKeyPrefix { get; private set; }
        public static int QuotaOfUidCount { get; private set; }
        public static int QuotaOfUidTime { get; private set; }

        public static void Set(IConfiguration configuration)
        {
            BaseUrl = configuration.GetRequiredSection("External:SmsSettings:BaseUrl").Value;
            ApiKey = configuration.GetRequiredSection("External:SmsSettings:ApiKey").Value;
            ApiKeyPrefix = configuration.GetRequiredSection("External:SmsSettings:ApiKeyPrefix").Value;
            QuotaOfUidCount = int.Parse(configuration.GetRequiredSection("External:SmsSettings:QuotaOfUidCount").Value);
            QuotaOfUidTime = int.Parse(configuration.GetRequiredSection("External:SmsSettings:QuotaOfUidTime").Value);
        }
    }
}
