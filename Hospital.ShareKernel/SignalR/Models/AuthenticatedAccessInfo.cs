namespace Hospital.SharedKernel.SignalR.Models
{
    public class AuthenticatedAccessInfo : BaseAccessInfo
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Fullname { get; set; }
    }
}
