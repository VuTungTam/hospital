using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Symptoms
{
    public interface IBookingReadRepository : IReadRepository<Booking>
    {
        Task<PaginationResult<Booking>> GetMyListPagingWithFilterAsync(Pagination pagination, BookingStatus status, long serviceId = 0, DateTime date = default, CancellationToken cancellationToken = default);

        Task<PaginationResult<Booking>> GetPagingWithFilterAsync(Pagination pagination, BookingStatus status, long excludeId = 0, DateTime date = default, long ownerId = 0, CancellationToken cancellationToken = default);

        Task<int> GetMaxOrderAsync(long serviceId, DateTime date, long timeSlotId, CancellationToken cancellationToken);

        Task<int> GetCurrentAsync(long serviceId, long timeSlotId, CancellationToken cancellationToken);

        //Task<int> GetBookingQuantity(long? serviceId, DateTime? date, TimeSpan? start, TimeSpan? end, List<BookingStatus>? status, CancellationToken cancellationToken);
    }
}
