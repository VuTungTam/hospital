using Hospital.Domain.Entities.Payments;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Payments
{
    public interface IPaymentReadRepository : IReadRepository<Payment>
    {
    }
}
