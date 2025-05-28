using Hospital.Application.Dtos.Bookings;
using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Bookings
{
    public class CheckCurrentBookingQuery : BaseQuery<bool>
    {
        public CheckCurrentBookingQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
