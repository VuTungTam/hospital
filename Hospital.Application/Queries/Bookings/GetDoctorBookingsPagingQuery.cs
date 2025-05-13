using Hospital.Application.Dtos.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Bookings
{
    public class GetDoctorBookingsPagingQuery : BaseQuery<PaginationResult<BookingDto>>
    {
        public GetDoctorBookingsPagingQuery(Pagination pagination, BookingStatus status, long serviceId, long timeSlotId, DateTime date, long ownerId)
        {
            Pagination = pagination;
            OwnerId = ownerId;
            ServiceId = serviceId;
            TimeSlotId = timeSlotId;
            Status = status;
            Date = date;
        }

        public Pagination Pagination { get; }
        public long OwnerId { get; }
        public long ServiceId { get; }
        public long TimeSlotId { get; }
        public BookingStatus Status { get; }
        public DateTime Date { get; }
    }
}
