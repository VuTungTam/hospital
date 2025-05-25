using Hospital.Domain.Configs;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Configures.Models;
using Hospital.SharedKernel.CoreConfigs;
using Hospital.SharedKernel.Infrastructure.ExternalServices.VNPay.Models;
using Hospital.SharedKernel.Infrastructure.Redis;

namespace Hospital.Api
{
    public class DefaultConfigSetup
    {
        public static void Exec(IConfiguration configuration)
        {
            var delayProcessMilliseconds = configuration["PowerfulSetting:DelayProcessMilliSeconds"];

            //PowerfulSetting.Password = configuration["PowerfulSetting:Password"];

            //PowerfulSetting.DelayProcessMilliSeconds = string.IsNullOrEmpty(delayProcessMilliseconds) ? 0 : int.Parse(delayProcessMilliseconds);

            JwtSettingsConfig.Set(configuration);

            FeatureConfig.Set(configuration);

            //DbConfig.Set(configuration);

            AuthConfig.Set(configuration);

            CdnConfig.Set(configuration);

            ClientInfoConfig.Set(configuration);

            RedisConfig.Set(configuration);

            VNPayConfig.Set(configuration);

            //GoogleMapsConfig.Set(configuration);
        }
    }
}
