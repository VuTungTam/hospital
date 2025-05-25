using Hospital.Application.Dtos.Payments;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Payments
{
    public class GetPaymentByTransactionIdQuery : BaseQuery<PaymentDto>
    {
        public GetPaymentByTransactionIdQuery(long transactionId)
        {
            TransactionId = transactionId;
        }

        public long TransactionId { get; }
    }
}
