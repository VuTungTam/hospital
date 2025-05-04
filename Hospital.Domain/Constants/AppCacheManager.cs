using Hospital.Domain.Entities.Articles;
using Hospital.SharedKernel.Infrastructure.Caching.Models;

namespace Hospital.Domain.Constants
{
    public static class AppCacheManager
    {
        public static CacheEntry Script = new CacheEntry("script", 604800);

        public static CacheEntry SmsCounter = new CacheEntry("sms-counter", 600);

        public static CacheEntry TeamMember = new CacheEntry("team-member", 10800);

        public static CacheEntry Contact = new CacheEntry("contact", 259200);

        public static CacheEntry Services = new CacheEntry("services", 3600);

        public static CacheEntry GetSmsCountByUidCacheEntry(string uid) => new CacheEntry($"sms-counter:by-ip:{uid}", 3600);

        public static CacheEntry GetLoginOtpCacheEntry(string phone) => new CacheEntry($"login-otp:{phone}", 300);

        public static CacheEntry GetServiceByIdAndLangCacheEntry(long id, List<string> langs) => new CacheEntry($"services-by-id-and-langs:{id}:{string.Join(":", langs)}", 604800);

        public static CacheEntry GetFacilityBySlugAndLangsCacheEntry(string slug, List<string> langs) => new CacheEntry($"facility-by-slug-and-langs:{slug}:{string.Join(":", langs)}", 604800);

        public static CacheEntry GetServiceTypeBySlugAndLangsCacheEntry(string slug, List<string> langs) => new CacheEntry($"service-type-by-slug-and-langs:{slug}:{string.Join(":", langs)}", 604800);

        public static CacheEntry GetAricleBySlugCacheEntry(string slug) => new CacheEntry($"{typeof(Article).Name.ToLower()}:slug:{slug}", 604800);

        public static CacheEntry GetDistanceMatrixCacheEntry(double srcLat, double srcLng, double desLat, double desLng) => new CacheEntry($"distance-matrix:{srcLat}:{srcLng}:{desLat}:{desLng}", 8640000);

        public static CacheEntry GetNotificationCountCacheEntry(long userId) => new CacheEntry($"notifications:count:{userId}", 604800);
    }
}
