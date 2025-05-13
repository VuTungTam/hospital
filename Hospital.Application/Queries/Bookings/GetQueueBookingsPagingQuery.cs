using Hospital.Application.Dtos.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Bookings
{
    public class GetQueueBookingsPagingQuery : BaseQuery<PaginationResult<BookingDto>>
    {
        public GetQueueBookingsPagingQuery(Pagination pagination, long serviceId, long timeSlotId, DateTime date)
        {
            Pagination = pagination;
            ServiceId = serviceId;
            TimeSlotId = timeSlotId;
            Date = date;
        }

        public Pagination Pagination { get; }
        public long ServiceId { get; }
        public long TimeSlotId { get; }
        public DateTime Date { get; }
    }
}
