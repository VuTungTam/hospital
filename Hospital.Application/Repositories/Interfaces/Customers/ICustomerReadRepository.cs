using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Enums;

namespace Hospital.Application.Repositories.Interfaces.Customers
{
    public interface ICustomerReadRepository : IReadRepository<Customer>
    {
        Task<Customer> GetLoginByEmailAsync(string email, string password, bool checkPassword = true, CancellationToken cancellationToken = default);

        Task<Customer> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<Customer> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);

        Task<PaginationResult<Customer>> GetCustomersPaginationResultAsync(Pagination pagination, AccountStatus status = AccountStatus.None, CancellationToken cancellationToken = default);

        Task<List<Customer>> GetCustomerNamesAsync(CancellationToken cancellationToken = default);

        Task<List<Customer>> GetCustomesAsync(CancellationToken cancellationToken = default);
    }
}
