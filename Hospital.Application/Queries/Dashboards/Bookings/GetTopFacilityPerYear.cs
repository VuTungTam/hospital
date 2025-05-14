using Hospital.Application.Models.Dashboards.Bookings;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Dashboards.Bookings
{
    [RequiredPermission(ActionExponent.ViewDashboard)]
    public class GetTopFacilityPerYearQuery : BaseQuery<TopFacility>
    {
        public GetTopFacilityPerYearQuery(int year)
        {
            Year = year;
        }

        public int Year { get; }
    }
}
