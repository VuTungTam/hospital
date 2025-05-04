using Hospital.Domain.Entities.Payments;
using Hospital.SharedKernel.Application.Repositories.Interface;
using MediatR;

namespace Hospital.Application.Repositories.Interfaces.Payments
{
    public interface IPaymentWriteRepository : IWriteRepository<Payment>
    {
    }
}
