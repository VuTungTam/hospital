using Hospital.Domain.Entities.Feedbacks;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Feedbacks
{
    public interface IFeedbackReadRepository : IReadRepository<Feedback>
    {
        Task<Feedback> GetByBookingIdAsync(long bookingId, CancellationToken cancellationToken);
    }
}
