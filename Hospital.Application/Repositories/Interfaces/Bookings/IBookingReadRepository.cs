using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using MediatR;

namespace Hospital.Application.Repositories.Interfaces.Symptoms
{
    public interface IBookingReadRepository : IReadRepository<Booking>
    {
        Task<PagingResult<Booking>> GetMyListPagingWithFilterAsync(Pagination pagination, BookingStatus status, long serviceId = 0, DateTime date = default, CancellationToken cancellationToken = default);

        Task<PagingResult<Booking>> GetPagingWithFilterAsync(Pagination pagination, BookingStatus status, long excludeId = 0, DateTime date = default, long ownerId = 0, bool ignoreOwner = false, CancellationToken cancellationToken = default);

        Task<List<Booking>> GetNextBookingAsync(Booking booking, CancellationToken cancellationToken = default);

        Task<int> GetMaxOrderAsync(long serviceId, DateTime date, TimeSpan start, TimeSpan end, CancellationToken cancellationToken);

        Task<int> GetCurrentAsync(long serviceId, CancellationToken cancellationToken);

    }
}
