using Microsoft.Extensions.Configuration;

namespace Hospital.SharedKernel.Infrastructure.Services.Emails.Models
{
    public class EmailConfig
    {
        public static string Host { get; private set; }
        public static string AppPassword { get; private set; }
        public static int Port { get; private set; }
        public static string Sender { get; private set; }
        public static string DisplayName { get; private set; }

        public static void Set(IConfiguration configuration)
        {
            //Host = configuration.GetRequiredSection("EmailSettings:Host").Value;
            //AppPassword = configuration.GetRequiredSection("EmailSettings:AppPassword").Value;
            //Port = int.Parse(configuration.GetRequiredSection("EmailSettings:Port").Value);
            //Sender = configuration.GetRequiredSection("EmailSettings:Sender").Value;
            //DisplayName = configuration.GetRequiredSection("EmailSettings:DisplayName").Value;
        }
    }
}
