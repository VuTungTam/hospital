using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MassTransit.Internals;

namespace Hospital.SharedKernel.Infrastructure.Caching.Models
{
    public static class CacheManager
    {
        public static int DefaultExpiriesInSeconds = 300;


        public static CacheEntry SystemConfiguration = new CacheEntry("system-configuration", 86400);

        public static CacheEntry MasterAction = new CacheEntry("action:master", 86400);

        public static CacheEntry SuperAdmin = new CacheEntry("super-admin", 86400);

        public static string GetCombineKey(string key) => $"{RedisConfig.Prefix}:{key}";

        public static string GetTableName<T>() where T : BaseEntity => ((T)Activator.CreateInstance(typeof(T))).GetTableName();

        public static string GetConnectionSocketKey(long userId) => $"socket:{userId}";

        public static string GetRemovePaginationPattern<T>(long ownerId, long facilityId) where T : BaseEntity
        {
            var typeofT = typeof(T);
            var key = $"pagination:{GetTableName<T>()}";
            var isSystem = typeofT.HasInterface<ISystemEntity>();

            if (!isSystem && typeofT.HasInterface<IOwnedEntity>())
            {
                key = $"{key}:{ownerId}";
            }

            if (!isSystem && typeofT.HasInterface<IFacility>())
            {
                key = $"{key}:{facilityId}";
            }

            return key + "*";
        }

        public static CacheEntry GetLoginSecure(string uid) => new CacheEntry($"login-secure:{uid}", 180);

        public static CacheEntry GetPaginationCacheEntry<T>(Pagination pagination, long ownerId, long facilityId) where T : BaseEntity
        {
            var typeofT = typeof(T);
            var key = $"pagination:{GetTableName<T>()}";
            var suffix = $"{pagination.Page}:{pagination.Size}:{pagination.Search?.ToBase64Encode()}";
            var isSystem = typeofT.HasInterface<ISystemEntity>();
            var expires = 86400;

            if (!isSystem && typeofT.HasInterface<IOwnedEntity>())
            {
                key = $"{key}:{ownerId}";
            }

            if (!isSystem && typeofT.HasInterface<IFacility>())
            {
                key = $"{key}:{facilityId}";
            }

            key = $"{key}:{suffix}";

            return new CacheEntry(key, expires);
        }

        public static CacheEntry GetLockUpdateCacheEntry<T>(long id) where T : BaseEntity => new CacheEntry($"updating_lock_{typeof(T)}_{id}", 600);

        public static CacheEntry GetExcelCacheEntry(string name) => new CacheEntry(name, 1800);

        public static CacheEntry GetCodeVerifierCacheEntry(string codeChallenge) => new CacheEntry($"zalo:code-verifier:{codeChallenge}", 3600);

        public static CacheEntry GetSequenceCacheEntry(string table) => new CacheEntry($"sequence:{table}", 7200);

        public static CacheEntry GetAccessTokenCacheEntry(long userId) => new CacheEntry($"access-token:{userId}", AuthConfig.TokenTime);

        public static CacheEntry GetClientInformationCacheEntry(string ip) => new CacheEntry($"client-information:{ip}", 7776000);

        public static CacheEntry GetSecretCacheEntry(string keyName, long ownerId) => new CacheEntry($"{keyName}:{ownerId}", 60);

        public static CacheEntry GetS3CacheFileCacheEntry(string fileName) => new CacheEntry($"s3:{fileName}", 31536000);

        public static CacheEntry GetVerifyAccountCacheEntry(string email, string code) => new CacheEntry($"verify-account:{email}:{code}", 604800);

        public static CacheEntry GetBlockSendVerifyAccountCacheEntry(string email) => new CacheEntry($"block-send-verify-account:{email}", 300);

        public static CacheEntry GetForgotPwdCacheEntry(string email) => new CacheEntry($"forgot-pwd:{email}", 300);

        public static CacheEntry GetForgotPwdSessionCacheEntry(string email) => new CacheEntry($"forgot-pwd-session:{email}", 300);

        public static CacheEntry GetBlockSendForgotPwdCacheEntry(string email) => new CacheEntry($"block-send-forgot-pwd:{email}", 300);

        public static CacheEntry GetShortUrlCacheEntry(string code) => new CacheEntry($"short-rul:{code}", 7776000);

        public static CacheEntry GetMaxOrderCacheEntry(long serviceId, DateTime date, long timeSlotId) => new CacheEntry($"max-order:{serviceId}:{date:yyyyMMdd}:time-slot:{timeSlotId}", 7776000);

        public static CacheEntry GetTimeRulesEntry(long serviceId) => new CacheEntry($"service-time-rules:{serviceId}", 7776000);

        public static CacheEntry GetTimeSlotsEntry(long timeRuleId) => new CacheEntry($"time-slots:{timeRuleId}", 7776000);

        public static CacheEntry GetCurrentOrderCacheEntry(long serviceId, DateTime date, long timeSlotId) => new CacheEntry($"current-order:{serviceId}:{date:yyyyMMdd}:time-slot:{timeSlotId}", 1800);

        public static CacheEntry GetFacilityType() => new CacheEntry($"facility-types", 7776000);

        public static CacheEntry GetMostFacilities() => new CacheEntry($"most-facilities", 7776000);

        public static CacheEntry GetFacilityServiceType(long id) => new CacheEntry($"facility:{id}:service-types", 7776000);

        public static CacheEntry GetCustomerPermission() => new CacheEntry($"customer-permission", 7776000);

        public static CacheEntry GetDoctorPermission() => new CacheEntry($"doctor-permission", 7776000);

        public static CacheEntry GetDoctorContext(long doctorId) => new CacheEntry($"doctor-context:{doctorId}", 3600);

        public static CacheEntry GetBookingIdByCodeCacheEntry(string code)
                    => new CacheEntry($"id:s-row-code:tbl_booking:{code}", 3600);



        public static CacheEntry DbSystemPublicIdCacheEntry<T>(long id) where T : BaseEntity
            => new CacheEntry($"{typeof(T).Name.ToLower()}:s-row-id:public-{GetTableName<T>()}:{id}", 3600);

        public static CacheEntry DbSystemPublicAllCacheEntry<T>() where T : BaseEntity
                   => new CacheEntry($"{typeof(T).Name.ToLower()}:s-rows:public-{GetTableName<T>()}", 3600);

        public static CacheEntry DbSystemIdCacheEntry<T>(long id) where T : BaseEntity
            => new CacheEntry($"{typeof(T).Name.ToLower()}:s-row-id:{GetTableName<T>()}:{id}", 3600);

        public static CacheEntry DbOwnerIdCacheEntry<T>(long recordId, long ownerId) where T : BaseEntity
            => new CacheEntry($"{typeof(T).Name.ToLower()}:row-id:{GetTableName<T>()}:{recordId}:{ownerId}", 600);

        public static CacheEntry DbSystemAllCacheEntry<T>() where T : BaseEntity
            => new CacheEntry($"{typeof(T).Name.ToLower()}:s-rows:{GetTableName<T>()}", 3600);


        public static CacheEntry DbOwnerAllCacheEntry<T>(long ownerId) where T : BaseEntity
            => new CacheEntry($"{typeof(T).Name.ToLower()}:rows:{GetTableName<T>()}:{ownerId}", 600);

        public static CacheEntry GetAllProfileCacheEntry(long ownerId)
            => new CacheEntry($"health-profiles:{ownerId}", 3600);

        public static CacheEntry GetProfileCacheEntry(long id, long ownerId)
            => new CacheEntry($"health-profile:{id}:{ownerId}", 3600);
    }
}