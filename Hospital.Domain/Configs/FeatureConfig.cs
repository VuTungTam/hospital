using Microsoft.Extensions.Configuration;

namespace Hospital.Domain.Configs
{
    public static class FeatureConfig
    {
        public static bool Employee { get; set; }
        public static bool Customer { get; set; }
        public static bool Symptom { get; set; }
        public static bool Booking { get; set; }
        public static bool SystemSetting { get; set; }
        public static bool Audit { get; set; }
        public static bool Decentralization { get; set; }

        public static void SetConfig(IConfiguration configuration)
        {
            Employee = bool.Parse(configuration.GetRequiredSection("Features:Employee").Value);
            Customer = bool.Parse(configuration.GetRequiredSection("Features:Customer").Value);
            Symptom = bool.Parse(configuration.GetRequiredSection("Features:Symptom").Value);
            Booking = bool.Parse(configuration.GetRequiredSection("Features:Booking").Value);
            SystemSetting = bool.Parse(configuration.GetRequiredSection("Features:SystemSetting").Value);
            Audit = bool.Parse(configuration.GetRequiredSection("Features:Audit").Value);
            Decentralization = bool.Parse(configuration.GetRequiredSection("Features:Decentralization").Value);
        }

        public static object Get()
        {
            return new
            {
                Employee,
                Customer,
                Symptom,
                Booking,
                SystemSetting,
                Audit,
                Decentralization
            };
        }
    }
}
