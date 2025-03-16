using Microsoft.Extensions.Configuration;

namespace Hospital.SharedKernel.Infrastructure.Redis
{
    public class RedisConfig
    {
        public static string Prefix { get; private set; }

        public static string Host { get; private set; }

        public static int Port { get; private set; }

        public static string Password { get; private set; }

        public static int DbNumber { get; private set; }

        public static int Timeout { get; private set; }

        public static void Set(IConfiguration configuration)
        {
            Prefix = configuration.GetRequiredSection("Caching:Prefix").Value;
            Host = configuration.GetRequiredSection("Caching:Redis:Host").Value;
            Password = configuration.GetRequiredSection("Caching:Redis:Password").Value;
            Port = int.Parse(configuration.GetRequiredSection("Caching:Redis:Port").Value);
            DbNumber = int.Parse(configuration.GetRequiredSection("Caching:Redis:DbNumber").Value);
            Timeout = 
                int.Parse(configuration.GetRequiredSection("Caching:Redis:Timeout").Value);
        }
    }
}
