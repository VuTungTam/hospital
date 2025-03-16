using Microsoft.Extensions.Configuration;

namespace Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Models
{
    public class GoogleMapsConfig
    {
        public static string Url { get; private set; }

        public static string ApiKey { get; private set; }

        public static void Set(IConfiguration configuration)
        {
            Url = configuration.GetRequiredSection("External:GoogleMaps:Url").Value;
            ApiKey = configuration.GetRequiredSection("External:GoogleMaps:ApiKey").Value;
        }
    }
}
