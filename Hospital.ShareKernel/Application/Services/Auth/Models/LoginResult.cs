namespace Hospital.SharedKernel.Application.Services.Auth.Models
{
    public class LoginResult
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn => AuthConfig.TokenTime;

        public string TokenType => "Bearer";

        public bool IsPasswordChangeRequired { get; set; }
    }
}
