using Microsoft.Extensions.Configuration;

namespace Hospital.SharedKernel.CoreConfigs
{

    public class ClientInfoConfig
    {
        public static string Url { get; private set; }

        public static void SetConfig(IConfiguration configuration)
        {
            Url = configuration.GetRequiredSection("ClientInfo:Url").Value;
        }
    }
}
