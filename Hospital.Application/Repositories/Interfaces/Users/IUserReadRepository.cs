using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Enums;

namespace Hospital.Application.Repositories.Interfaces.Users
{
    public interface IUserReadRepository : IReadRepository<User>
    {
        Task<bool> PhoneExistAsync(string phone, long exceptId = 0, CancellationToken cancellationToken = default);

        Task<bool> EmailExistAsync(string email, long exceptId = 0, CancellationToken cancellationToken = default);

        Task<bool> CodeExistAsync(string code, long exceptId = 0, CancellationToken cancellationToken = default);

        Task<List<User>> GetSuperAdminsAsync(CancellationToken cancellationToken);

        Task<User> GetCurrentUserAsync(bool includeRole = false, CancellationToken cancellationToken = default);

        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<User> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);

        Task<User> GetByIdIncludedRolesAsync(long id, CancellationToken cancellationToken = default);

        Task<PagingResult<User>> GetEmployeesPagingResultAsync(Pagination pagination, AccountStatus state = AccountStatus.None, long roleId = 0, CancellationToken cancellationToken = default);

        Task<PagingResult<User>> GetCustomersPagingResultAsync(Pagination pagination, AccountStatus state = AccountStatus.None, CancellationToken cancellationToken = default);

        Task<List<User>> GetCustomerNamesAsync(CancellationToken cancellationToken = default);
    }
}
