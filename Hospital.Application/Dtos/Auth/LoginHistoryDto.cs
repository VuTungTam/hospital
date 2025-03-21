namespace Hospital.Application.Dtos.Auth
{
    public class LoginHistoryDto : BaseDto
    {
        public long UserId { get; set; }

        public DateTime Timestamp { get; set; }

        public string Ip { get; set; }

        public string UA { get; set; }

        public string Origin { get; set; }
    }
}
