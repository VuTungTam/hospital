using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Bookings
{
    public interface IBookingWriteRepository : IWriteRepository<Booking>
    {
        Task ActionAfterAddAsync(Booking booking, CancellationToken cancellationToken);

        Task AddBookingCodeAsync(Booking booking, CancellationToken cancellationToken);

        Task ChangeStatusAsync(long bookingId, BookingStatus status, CancellationToken cancellationToken);

        Task ClearCacheAsync(Booking booking, CancellationToken cancellationToken);

        Task ScheduleNotificationForCustomerAsync(long bookingId, string code, DateTime appointmentDate, TimeSpan startTime, CancellationToken cancellationToken);

        void TestSchedule();
    }
}
