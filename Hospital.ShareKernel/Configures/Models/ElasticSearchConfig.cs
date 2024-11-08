namespace Hospital.SharedKernel.Configures.Models
{
    public class ElasticSearchConfig
    {
        public static string Uri { get; set; }
        public static string ApplicationName { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static bool Enabled { get; set; }
    }
}
