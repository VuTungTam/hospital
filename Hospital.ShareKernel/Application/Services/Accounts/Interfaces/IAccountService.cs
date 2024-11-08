using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Domain.Entities.Users;

namespace Hospital.SharedKernel.Application.Services.Accounts.Interfaces
{
    public interface IAccountService
    {
        Task SendVerifyEmailAsync(User user, string code, CancellationToken cancellationToken);

        Task SendVerifyEmailWithPasswordAsync(User user, string code, CancellationToken cancellationToken);

        Task SendForgotPwdAsync(User user, string code, CancellationToken cancellationToken);

        Task SendChangePasswordNoticeAsync(User user, RequestInfo requestInfo, CancellationToken cancellationToken);

        Task SendPasswordForUserAsync(User user, CancellationToken cancellationToken);
    }
}
