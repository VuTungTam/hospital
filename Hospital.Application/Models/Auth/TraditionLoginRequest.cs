namespace Hospital.Application.Models.Auth
{
    public class TraditionLoginRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string OTP { get; set; }
    }
}
