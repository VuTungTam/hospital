using Hospital.Application.Dtos.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Bookings
{
    public class GetMyListBookingsPagingQuery : BaseQuery<PaginationResult<BookingResponseDto>>
    {
        public GetMyListBookingsPagingQuery(Pagination pagination, BookingStatus status, long serviceId, DateTime date)
        {
            Pagination = pagination;
            Status = status;
            ServiceId = serviceId;
            Date = date;
        }

        public Pagination Pagination { get; }
        public BookingStatus Status { get; }
        public long ServiceId { get; }
        public DateTime Date { get; }
    }
}
