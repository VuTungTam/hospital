using Microsoft.Extensions.Configuration;

namespace Hospital.SharedKernel.Caching.Models
{
    public class CachingConfig
    {
        public static string Prefix { get; private set; }

        public static string Host { get; private set; }

        public static int Port { get; private set; }

        public static string Password { get; private set; }

        public static int DbNumber { get; private set; }

        public static void SetConfig(IConfiguration configuration)
        {
            Prefix = configuration.GetRequiredSection("Caching:Prefix").Value;
            Host = configuration.GetRequiredSection("Caching:Redis:Host").Value;
            Password = configuration.GetRequiredSection("Caching:Redis:Password").Value;
            Port = int.Parse(configuration.GetRequiredSection("Caching:Redis:Port").Value);
            DbNumber = int.Parse(configuration.GetRequiredSection("Caching:Redis:DbNumber").Value);
        }
    }
}
