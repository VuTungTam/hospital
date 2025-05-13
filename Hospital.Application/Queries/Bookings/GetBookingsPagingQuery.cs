using Hospital.Application.Dtos.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Bookings
{
    public class GetBookingsPagingQuery : BaseQuery<PaginationResult<BookingDto>>
    {
        public GetBookingsPagingQuery(Pagination pagination, long userId, BookingStatus status, DateTime date, long excludeId)
        {
            Pagination = pagination;
            UserId = userId;
            Status = status;
            Date = date;
            ExcludeId = excludeId;
        }

        public Pagination Pagination { get; }
        public long UserId { get; }
        public BookingStatus Status { get; }
        public DateTime Date { get; }
        public long ExcludeId { get; }
    }
}
