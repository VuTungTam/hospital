using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Enums;

namespace Hospital.Application.Repositories.Interfaces.Users
{
    public interface IUserWriteRepository : IWriteRepository<User>
    {
        Task AddEmployeeAsync(User user, CancellationToken cancellationToken);

        Task AddCustomerAsync(User user, bool externalFlow = false, AccountStatus status = AccountStatus.UnConfirm, CancellationToken cancellationToken = default);

        Task UpdateStatusAsync(User user, CancellationToken cancellationToken);

        Task UpdateRolesAsync(long userId, IEnumerable<long> roleIds, CancellationToken cancellationToken);
    }
}
