using Hospital.Application.Models.Dashboards.Bookings;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Dashboards.Bookings
{
    [RequiredPermission(ActionExponent.ViewDashboard)]
    public class GetYearlyFacilityBookingStatsQuery : BaseQuery<YearlyFacilityBookingStats>
    {
        public GetYearlyFacilityBookingStatsQuery(int year, bool isOnlyCompleted)
        {
            Year = year;
            IsOnlyCompleted = isOnlyCompleted;
        }

        public int Year { get; }
        public bool IsOnlyCompleted { get; }
    }
}
