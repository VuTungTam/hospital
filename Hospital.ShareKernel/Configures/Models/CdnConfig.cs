using Microsoft.Extensions.Configuration;

namespace Hospital.SharedKernel.Configures.Models
{
    public static class CdnConfig
    {
        public static string CloudName { get; set; }

        public static string ApiKey { get; set; }

        public static string ApiSecret { get; set; }

        public static string BaseUrl => $"https://res.cloudinary.com/{CloudName}/image/upload";

        public static void SetConfig(IConfiguration configuration)
        {
            CloudName = configuration.GetRequiredSection("Cloudinary:CloudName").Value;
            ApiKey = configuration.GetRequiredSection("Cloudinary:ApiKey").Value;
            ApiSecret = configuration.GetRequiredSection("Cloudinary:ApiSecret").Value;
        }

        public static string Get(string name)
        {
            return $"{BaseUrl}/{name}";
        }
    }

}
