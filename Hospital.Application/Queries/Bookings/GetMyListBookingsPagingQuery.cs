using Hospital.Application.Dtos.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Bookings
{
    [RequiredPermission(ActionExponent.BookingManagement)]
    public class GetMyListBookingsPagingQuery : BaseQuery<PaginationResult<BookingDto>>
    {
        public GetMyListBookingsPagingQuery(Pagination pagination, BookingStatus status, long serviceTypeId, long serviceId, DateTime date)
        {
            Pagination = pagination;
            Status = status;
            ServiceId = serviceId;
            ServiceTypeId = serviceTypeId;
            Date = date;
        }

        public Pagination Pagination { get; }
        public BookingStatus Status { get; }
        public long ServiceTypeId { get; }
        public long ServiceId { get; }
        public DateTime Date { get; }
    }
}
