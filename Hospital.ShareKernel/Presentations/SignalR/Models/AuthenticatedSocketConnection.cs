namespace Hospital.SharedKernel.Presentations.SignalR.Models
{
    public class AuthenticatedSocketConnection : SocketConnection
    {
        public string UserId { get; set; }

        public long FacilityId { get; set; }

        public long ZoneId { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }
    }
}
