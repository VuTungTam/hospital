using Hospital.Domain.Entities.CancelReasons;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.CancelReasons
{
    public interface ICancelReasonReadRepository : IReadRepository<CancelReason>
    {
        Task<CancelReason> GetByBookingIdAsync(long bookingId, CancellationToken cancellationToken);
    }
}