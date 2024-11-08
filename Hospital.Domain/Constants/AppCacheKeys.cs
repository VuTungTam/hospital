using Hospital.Domain.Entities.Newses;

namespace Hospital.Domain.Constants
{
    public static class AppCacheKeys
    {
        public static string GetScriptKey() => "script";

        public static string GetSmsCounterKey() => "sms-counter";

        public static string GetSmsCountByUidKey(string uid) => $"sms-counter:by-ip:{uid}";

        public static string GetLoginOtpKey(string phone) => $"login-otp:{phone}";

        public static string GetTeamInfoKey() => "team-info";

        public static string GetContactKey() => "contact";

        public static string GetServicesKey() => "services";

        public static string GetServiceByIdAndLangKey(long id, List<string> langs) => $"services-by-id-and-langs:{id}:{string.Join(":", langs)}";

        public static string GetNewsBySlugKey(string slug) => $"{typeof(News).Name.ToLower()}:slug:{slug}";
    }
}
