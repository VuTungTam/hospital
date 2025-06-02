using Hospital.Application.Dtos.TimeSlots;
using Hospital.Application.Models.Bookings;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Bookings
{
    public interface IBookingReadRepository : IReadRepository<Booking>
    {
        Task<PaginationResult<Booking>> GetMyListPagingWithFilterAsync(Pagination pagination, BookingStatus status, long serviceTypeId = 0, long serviceId = 0, DateTime date = default, CancellationToken cancellationToken = default);

        Task<PaginationResult<Booking>> GetPagingWithFilterAsync(Pagination pagination, BookingStatus status, long excludeId = 0, DateTime date = default, long ownerId = 0, CancellationToken cancellationToken = default);

        Task<PaginationResult<Booking>> GetDoctorPagingWithFilterAsync(Pagination pagination, BookingStatus status, long serviceId, long timeSlotId, DateTime date = default, long ownerId = 0, CancellationToken cancellationToken = default);

        Task<PaginationResult<Booking>> GetBookingInQueueAsync(Pagination pagination, long serviceId, long timeSlotId, DateTime date = default, CancellationToken cancellationToken = default);

        Task<int> GetMaxOrderAsync(long serviceId, DateTime date, long timeSlotId, CancellationToken cancellationToken);

        Task<CurrentBookingModel> GetCurrentAsync(long serviceId, long timeSlotId, CancellationToken cancellationToken);

        Task<Booking> GetByCodeAsync(string code, CancellationToken cancellationToken);

        Task<List<Booking>> GetNextBookings(Booking cancelBooking, CancellationToken cancellationToken);

        Task<int> GetBookingCountByTimeSlotId(long timeSlotId, DateTime date, CancellationToken cancellationToken);

    }
}
