using Hospital.Application.Models.Dashboards.Services;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Dashboards.HealthServices
{
    [RequiredPermission(ActionExponent.ViewDashboard)]
    public class GetYearlyServiceStatsQuery : BaseQuery<YearlyServiceStats>
    {
        public GetYearlyServiceStatsQuery(long facilityId, int year, int top, bool isOnlyCompleted)
        {
            FacilityId = facilityId;
            Year = year;
            Top = top;
            IsOnlyCompleted = isOnlyCompleted;
        }

        public long FacilityId { get; }
        public int Year { get; }
        public int Top { get; }
        public bool IsOnlyCompleted { get; }
    }
}
