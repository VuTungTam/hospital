using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Models;

namespace Hospital.Application.Commands.Auth.RefreshTokens
{
    public class RefreshTokenCommand : BaseAllowAnonymousCommand<LoginResult>
    {
        public long UserId { get; set; }
        public string RefreshToken { get; set; }
        public RefreshTokenCommand(long userId, string refreshToken)
        {
            UserId = userId;
            RefreshToken = refreshToken;
        }
    }
}
