using Hospital.Application.Models.Dashboards.Articles;
using Hospital.Application.Models.Dashboards.Bookings;
using Hospital.Application.Models.Dashboards.Customers;
using Hospital.Application.Models.Dashboards.Services;

namespace Hospital.Application.Repositories.Interfaces.Dashboards
{
    public interface IDashboardRepository
    {
        Task<MonthlyBookingStatsPerYear> GetMonthlyBookingStatsPerYearAsync(long facilityId, int year, bool isOnlyCompleted, CancellationToken cancellationToken);

        Task<YearlyFacilityBookingStats> GetYearlyFacilityBookingStatsAsync(int year, bool isOnlyCompleted, CancellationToken cancellationToken);

        Task<YearlyServiceStats> GetYearlyServiceStatsAsync(long facilityId, int year, bool isOnlyCompleted, int top, CancellationToken cancellationToken);

        Task<ArticleStats> GetArticleStatsAsync(int top, CancellationToken cancellationToken);

        Task<BookingTrend> GetBookingTrendAsync(long facilityId, CancellationToken cancellationToken);

        Task<CustomerTrend> GetCustomerTrendAsync(CancellationToken cancellationToken);

        Task<BookingStatusStats> GetBookingStatusStatsAsync(long facilityId, int year, CancellationToken cancellationToken);

        Task<TopFacility> GetTopFacilityPerYear(int year, CancellationToken cancellationToken);
    }
}
