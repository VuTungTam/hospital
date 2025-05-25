using Hospital.Domain.Entities.Payments;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Payments
{
    public interface IPaymentReadRepository : IReadRepository<Payment>
    {
        Task<Payment> GetByTransactionId(long transactionId, CancellationToken cancellationToken);
    }
}
