using Hospital.SharedKernel.Caching.Models;
using Hospital.SharedKernel.Domain.Entities.Base;

namespace Hospital.SharedKernel.Application.Consts
{
    public class BaseCacheKeys { 
    public static string GetCombineKey(string key) => $"{CachingConfig.Prefix}:{key}";

    private static string GetTableName<T>() where T : BaseEntity => ((T)Activator.CreateInstance(typeof(T))).GetTableName();

    public static string GetSystemConfigurationKey() => $"system-configuration";

    public static string GetMasterActionKey() => "action:master";

    public static string GetCodeVerifierKey(string codeChallenge) => $"zalo:code-verifier:{codeChallenge}";

    public static string GetSequenceKey(string table) => $"sequence:{table}";

    public static string GetSocketKey(long userId) => $"socket:{userId}";

    public static string GetAccessTokenKey(long userId) => $"access-token:{userId}";

    public static string GetClientInformationKey(string ip) => $"client-information:{ip}";

    public static string GetSecretKey(string keyName, long ownerId) => $"{keyName}:{ownerId}";

    public static string GetS3CacheFileKey(string fileName) => $"s3:{fileName}";

    public static string GetVerifyAccountKey(string email, string code) => $"verify-account:{email}:{code}";

    public static string GetBlockSendVerifyAccountKey(string email) => $"block-send-verify-account:{email}";

    public static string GetSuperAdminKey() => "super-admin";

    public static string GetForgotPwdKey(string email) => $"forgot-pwd:{email}";

    public static string GetForgotPwdSessionKey(string email) => $"forgot-pwd-session:{email}";

    public static string GetBlockSendForgotPwdKey(string email) => $"block-send-forgot-pwd:{email}";

    public static string DbSystemIdKey<T>(long id) where T : BaseEntity
        => $"{typeof(T).Name.ToLower()}:s-row-id:{GetTableName<T>()}:{id}";

    public static string DbOwnerIdKey<T>(long recordId, long ownerId) where T : BaseEntity
        => $"{typeof(T).Name.ToLower()}:row-id:{GetTableName<T>()}:{recordId}:{ownerId}";

    public static string DbSystemAllKey<T>() where T : BaseEntity
        => $"{typeof(T).Name.ToLower()}:s-rows:{GetTableName<T>()}";

    public static string DbOwnerAllKey<T>(long ownerId) where T : BaseEntity
        => $"{typeof(T).Name.ToLower()}:rows:{GetTableName<T>()}:{ownerId}";
}
}
