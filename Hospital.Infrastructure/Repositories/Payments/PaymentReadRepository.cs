using Hospital.Application.Repositories.Interfaces.Payments;
using Hospital.Domain.Entities.Payments;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Payments
{
    public class PaymentReadRepository : ReadRepository<Payment>, IPaymentReadRepository
    {
        public PaymentReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<Payment> GetByTransactionId(long transactionId, CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.TransactionId == transactionId, cancellationToken);
        }
    }
}