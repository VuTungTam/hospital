using Hospital.Application.Models.Dashboards.Bookings;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Dashboards.Bookings
{
    [RequiredPermission(ActionExponent.ViewDashboard)]
    public class GetBookingDashboardByStatusQuery : BaseQuery<BookingStatusStats>
    {
        public GetBookingDashboardByStatusQuery(int year, long facilityId)
        {
            Year = year;
            FacilityId = facilityId;
        }

        public int Year { get; }
        public long FacilityId { get; }
    }
}
