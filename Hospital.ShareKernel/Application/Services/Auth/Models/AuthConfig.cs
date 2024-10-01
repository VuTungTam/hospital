using Microsoft.Extensions.Configuration;

namespace Hospital.SharedKernel.Application.Services.Auth.Models
{
    public class AuthConfig
    {
        public static int TokenTime { get; private set; } // Seconds
        public static int RefreshTokenTime { get; private set; } // Seconds
        public static int PasswordMinLength { get; private set; }
        public static int PasswordMaxLength { get; private set; }
        public static int OtpLength { get; private set; }
        public static int OtpAvaiableIn { get; private set; } // Minutes
        public static int ResendOtpAfter { get; private set; } // Minutes
        public static string IpInfoTokenKey { get; private set; }
        public static AuthIntegrationConfig Integration { get; private set; }

        public static void Set(IConfiguration configuration)
        {
            TokenTime = int.Parse(configuration.GetRequiredSection("Auth:Config:TokenTime").Value);
            RefreshTokenTime = int.Parse(configuration.GetRequiredSection("Auth:Config:RefreshTokenTime").Value);
            PasswordMinLength = int.Parse(configuration.GetRequiredSection("Auth:Config:PasswordMinLength").Value);
            PasswordMaxLength = int.Parse(configuration.GetRequiredSection("Auth:Config:PasswordMaxLength").Value);
            OtpLength = int.Parse(configuration.GetRequiredSection("Auth:Config:OtpLength").Value);
            OtpAvaiableIn = int.Parse(configuration.GetRequiredSection("Auth:Config:OtpAvaiableIn").Value);
            ResendOtpAfter = int.Parse(configuration.GetRequiredSection("Auth:Config:ResendOtpAfter").Value);
            IpInfoTokenKey = configuration.GetRequiredSection("Auth:Config:IpInfoTokenKey").Value;

            var integration = new AuthIntegrationConfig();
            configuration.GetRequiredSection("Auth:Integration").Bind(integration);

            Integration = integration;
        }
        public class AuthIntegrationConfig { }
    }
}
