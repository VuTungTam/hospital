using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Entities.Users;

namespace Hospital.SharedKernel.Application.Services.Accounts.Interfaces
{
    public interface IAccountService
    {
        Task SendVerifyEmailAsync(BaseUser user, string code, CancellationToken cancellationToken);

        Task SendVerifyEmailWithPasswordAsync(BaseUser user, string code, CancellationToken cancellationToken);

        Task SendForgotPwdAsync(BaseUser user, string code, CancellationToken cancellationToken);

        Task SendChangePasswordNoticeAsync(BaseUser user, RequestInfo requestInfo, CancellationToken cancellationToken);

        Task SendPasswordForUserAsync(BaseUser user, CancellationToken cancellationToken);
    }
}
