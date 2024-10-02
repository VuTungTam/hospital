using Hospital.SharedKernel.Application.Enums;

namespace Hospital.SharedKernel.Application.Services.Auth.Models
{
    public class AppToken
    {
        public string Value { get; set; }

        public DateTime Expires { get; set; }

        public TokenStatus Status { get; set; } = TokenStatus.None;
    }
}
