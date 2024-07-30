namespace Hospital.Domain.Constants
{
    /// <summary>
    /// Thời gian cache tính bằng seconds
    /// </summary>
    public class AppCacheTime
    {
        public const int LockUpdate = 60;
        public const int RecordWithId = 3600;
        public const int Records = 3600;

        public const int ForgotPasswordSession = 300;
        public const int LockSendingForgotPasswordWithEmail = 300;
        public const int LockSendingVerifyAccountWithEmail = 300;
        public const int SmsCounter = 600;
        public const int GoogleLoginCallback = 900;
        public const int LoginWithPhoneUsingOtp = 900;
        public const int ZaloChallengeCode = 3600;
        public const int ForgotPasswordCode = 3600;
        public const int Sequence = 7200;
        public const int TeamInfo = 10800;
        public const int SuperAdmin = 21600;
        public const int MasterAction = 43200;
        public const int SystemConfiguration = 86400;
        public const int Tenant = 86400;
        public const int VerifyAccountWithEmailCode = 604800;
    }
}
