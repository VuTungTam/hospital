using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Users
{
    public class CheckAccessByPasswordQuery : BaseAllowAnonymousQuery<string>
    {
        public CheckAccessByPasswordQuery(string username, bool autoSendOtp) {
            Username = username;
            AutoSendOtp = autoSendOtp;
        }
        public string Username { get; }
        public bool AutoSendOtp { get; }
    }
}
