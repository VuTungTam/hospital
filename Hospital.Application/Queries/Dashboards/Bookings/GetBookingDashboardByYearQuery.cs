using Hospital.Application.Models.Dashboards.Bookings;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Dashboards.Bookings
{
    [RequiredPermission(ActionExponent.ViewDashboard)]
    public class GetBookingDashboardByYearQuery : BaseQuery<MonthlyBookingStatsPerYear>
    {
        public GetBookingDashboardByYearQuery(int year, long facilityId, bool isOnlyCompleted)
        {
            Year = year;
            FacilityId = facilityId;
            IsOnlyCompleted = isOnlyCompleted;
        }

        public int Year { get; }
        public long FacilityId { get; }
        public bool IsOnlyCompleted { get; }
    }
}
