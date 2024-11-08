using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Bookings
{
    public interface IBookingWriteRepository : IWriteRepository<Booking>
    {
        Task AddWithCodeAsync(Booking booking, CancellationToken cancellationToken);

    }
}
