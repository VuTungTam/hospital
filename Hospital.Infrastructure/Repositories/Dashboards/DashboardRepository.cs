using Hospital.Application.Models.Dashboards.Articles;
using Hospital.Application.Models.Dashboards.Bookings;
using Hospital.Application.Models.Dashboards.Customers;
using Hospital.Application.Models.Dashboards.Services;
using Hospital.Application.Repositories.Interfaces.Dashboards;
using Hospital.Domain.Entities.Articles;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.Articles;
using Hospital.Domain.Specifications.Bookings;
using Hospital.Domain.Specifications.Customers;
using Hospital.Infrastructure.EFConfigurations;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Infrastructure.Databases.Dapper;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using VetHospital.Domain.Specifications.Bookings;

namespace Hospital.Infrastructure.Repositories.Dashboards
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IExecutionContext _executionContext;
        private readonly IDbConnection _dbConnection;

        public DashboardRepository(
            AppDbContext dbContext,
            IExecutionContext executionContext,
            IDbConnection dbConnection
        )
        {
            _dbContext = dbContext;
            _executionContext = executionContext;
            _dbConnection = dbConnection;
        }

        public async Task<MonthlyBookingStatsPerYear> GetMonthlyBookingStatsPerYearAsync(long facilityId, int year, bool isOnlyCompleted, CancellationToken cancellationToken)
        {
            isOnlyCompleted = false;

            ISpecification<Booking> spec = new BookingByYearEqualsSpecification(year);

            var result = new MonthlyBookingStatsPerYear();
            var query = _dbContext.Bookings.AsNoTracking();

            if (isOnlyCompleted)
            {
                spec = spec.And(new GetBookingsByStatusSpecification(BookingStatus.Completed));
            }

            if (facilityId <= 0)
            {
                if (!_executionContext.IsSA)
                {
                    spec = spec.And(new LimitByFacilityIdSpecification<Booking>(_executionContext.FacilityId));
                }
            }
            else
            {
                if (!_executionContext.IsSA && _executionContext.FacilityId != facilityId)
                {
                    throw new ForbiddenException();
                }

                spec = spec.And(new LimitByFacilityIdSpecification<Booking>(facilityId));
            }

            var bookings = await query.Where(spec.GetExpression())
                                      .Select(x => new Booking { Id = x.Id, Date = x.Date, Status = x.Status })
                                      .ToListAsync(cancellationToken);
            if (!bookings.Any())
            {
                return result;
            }

            var groups = bookings.GroupBy(x => x.Date.Month).OrderBy(x => x.Key);

            for (int i = 1; i <= 12; i++)
            {
                var model = new MonthlyBookingStatsValue
                {
                    Month = i
                };
                var group = groups.FirstOrDefault(g => g.Key == i);

                if (group != null)
                {
                    model.TotalCount = group.Count();
                    model.CompletedCount = isOnlyCompleted ? model.TotalCount : group.Count(x => x.Status == BookingStatus.Completed);
                }

                result.MonthValues.Add(model);
            }

            return result;
        }

        public async Task<YearlyFacilityBookingStats> GetYearlyFacilityBookingStatsAsync(int year, bool isOnlyCompleted, CancellationToken cancellationToken)
        {
            isOnlyCompleted = false;

            ISpecification<Booking> spec = new BookingByYearEqualsSpecification(year);

            var result = new YearlyFacilityBookingStats();
            var query = _dbContext.Bookings.AsNoTracking();

            if (isOnlyCompleted)
            {
                spec = spec.And(new GetBookingsByStatusSpecification(BookingStatus.Completed));
            }

            if (!_executionContext.IsSA)
            {
                spec = spec.And(new LimitByFacilityIdSpecification<Booking>(_executionContext.FacilityId));
            }

            var bookings = await query.Where(spec.GetExpression())
                                      .Select(x => new Booking { Id = x.Id, Date = x.Date, Status = x.Status, FacilityId = x.FacilityId, FacilityNameVn = x.FacilityNameVn })
                                      .ToListAsync(cancellationToken);
            if (!bookings.Any())
            {
                return result;
            }

            var groups = bookings.GroupBy(x => x.FacilityId);

            foreach (var group in groups)
            {
                var bookingsByGroup = group.ToList();

                result.Values.Add(new YearlyFacilityBookingStatsValue
                {
                    FacilityId = group.Key,
                    FacilityName = bookingsByGroup[0].FacilityNameVn,
                    TotalCount = bookingsByGroup.Count,
                    CompletedCount = bookingsByGroup.Count(x => x.Status == BookingStatus.Completed)
                });
            }

            return result;
        }

        public async Task<YearlyServiceStats> GetYearlyServiceStatsAsync(long facilityId, int year, bool isOnlyCompleted, int top, CancellationToken cancellationToken)
        {
            isOnlyCompleted = false;

            ISpecification<Booking> spec = new BookingByYearEqualsSpecification(year);

            var result = new YearlyServiceStats();
            var query = _dbContext.Bookings.AsNoTracking();

            if (isOnlyCompleted)
            {
                spec = spec.And(new GetBookingsByStatusSpecification(BookingStatus.Completed));
            }

            if (facilityId <= 0)
            {
                if (!_executionContext.IsSA)
                {
                    spec = spec.And(new LimitByFacilityIdSpecification<Booking>(_executionContext.FacilityId));
                }
            }
            else
            {
                if (!_executionContext.IsSA && _executionContext.FacilityId != facilityId)
                {
                    throw new ForbiddenException();
                }
                spec = spec.And(new LimitByFacilityIdSpecification<Booking>(facilityId));
            }

            var bookings = await query.Where(spec.GetExpression())
                                      .Select(x => new Booking { Id = x.Id, Status = x.Status, ServiceId = x.ServiceId })
                                      .ToListAsync(cancellationToken);
            if (!bookings.Any())
            {
                return result;
            }

            var groups = bookings.GroupBy(x => x.ServiceId).OrderByDescending(x => x.Count());
            var services = await _dbContext.HealthServices.Select(x => new HealthService { Id = x.Id, NameVn = x.NameVn, NameEn = x.NameEn }).ToListAsync(cancellationToken);

            for (int i = 0; i < top; i++)
            {
                var group = groups.ElementAtOrDefault(i);
                var model = new YearlyServiceStatsValue();

                if (group != null)
                {
                    model.ServiceId = group.Key;
                    model.ServiceName = services.FirstOrDefault(x => x.Id == group.Key)?.NameVn ?? "";
                    model.ServiceNameEn = services.FirstOrDefault(x => x.Id == group.Key)?.NameEn ?? "";
                    model.TotalCount = group.Count();
                    model.CompletedCount = group.Count(x => x.Status == BookingStatus.Completed);
                }

                result.Values.Add(model);
            }

            return result;
        }

        public async Task<ArticleStats> GetArticleStatsAsync(int top, CancellationToken cancellationToken)
        {
            var spec = new ArticleByViewCountGreaterThanEqualsSpecification(0);
            var articles = await _dbContext.Articles
                                           .AsNoTracking()
                                           .Where(spec.GetExpression())
                                           .Select(x => new Article { Title = x.Title, ViewCount = x.ViewCount })
                                           .OrderByDescending(x => x.ViewCount)
                                           .Take(top)
                                           .ToListAsync(cancellationToken);
            var result = new ArticleStats();

            foreach (var article in articles)
            {
                result.Values.Add(new ArticleStatsValue
                {
                    Title = article.Title,
                    ViewCount = article.ViewCount
                });
            }

            return result;
        }

        public async Task<BookingTrend> GetBookingTrendAsync(long facilityId, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var toDate = new DateTime(now.Year, now.Month, 1);
            var endDate = now;

            var toDateLastMonth = toDate.AddMonths(-1);
            var endDateLastMonth = toDate.AddDays(-1);

            var spec = new BookingByDateGreaterThanOrEqualsSpecification(toDate)
                .And(new BookingByDateLessThanOrEqualsSpecification(endDate));

            var spec2 = new BookingByDateGreaterThanOrEqualsSpecification(toDateLastMonth)
                .And(new BookingByDateLessThanOrEqualsSpecification(endDateLastMonth));

            if (_executionContext.IsSA)
            {
                if (facilityId > 0)
                {
                    spec = spec.And(new LimitByFacilityIdSpecification<Booking>(facilityId));
                    spec2 = spec2.And(new LimitByFacilityIdSpecification<Booking>(facilityId));
                }
            }
            else
            {
                var targetFacilityId = facilityId > 0 ? facilityId : _executionContext.FacilityId;
                spec = spec.And(new LimitByFacilityIdSpecification<Booking>(targetFacilityId));
                spec2 = spec2.And(new LimitByFacilityIdSpecification<Booking>(targetFacilityId));
            }

            var query = _dbContext.Bookings.Where(spec.GetExpression());
            var query2 = _dbContext.Bookings.Where(spec2.GetExpression());

            var thisMonthBookingCount = await query.CountAsync(cancellationToken);
            var lastMonthBookingCount = await query2.CountAsync(cancellationToken);

            return new BookingTrend
            {
                Value = thisMonthBookingCount,
                SamePeriodLastYearValue = lastMonthBookingCount,
                CompareExplain = $"So sánh [{toDateLastMonth:dd/MM/yyyy} - {endDateLastMonth:dd/MM/yyyy}] với [{toDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}]",
                CompareExplainEn = $"Compare [{toDateLastMonth:dd/MM/yyyy} - {endDateLastMonth:dd/MM/yyyy}] and [{toDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}]"
            };
        }


        public async Task<CustomerTrend> GetCustomerTrendAsync(CancellationToken cancellationToken)
        {
            var toDate = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
            var endDate = DateTime.Now;

            var toDateSamePeriodLastYear = toDate.AddYears(-1);
            var endDateSamePeriodLastYear = endDate.AddYears(-1);

            var spec = new CustomerByCreatedAtGreaterThanOrEqualsSpecification(toDate)
                  .And(new CustomerByCreatedAtLessThanOrEqualsSpecification(endDate));

            var spec2 = new CustomerByCreatedAtGreaterThanOrEqualsSpecification(toDateSamePeriodLastYear)
                   .And(new CustomerByCreatedAtLessThanOrEqualsSpecification(endDateSamePeriodLastYear));

            var query = _dbContext.Customers.Where(spec.GetExpression());
            var query2 = _dbContext.Customers.Where(spec2.GetExpression());

            var thisYearBookingCount = await query.CountAsync(cancellationToken);
            var lastYearBookingCount = await query2.CountAsync(cancellationToken);

            return new CustomerTrend
            {
                Value = thisYearBookingCount,
                SamePeriodLastYearValue = lastYearBookingCount,
                CompareExplain = $"So sánh [{toDateSamePeriodLastYear:dd/MM/yyyy} - {endDateSamePeriodLastYear:dd/MM/yyyy}] với [{toDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}]",
                CompareExplainEn = $"Compare [{toDateSamePeriodLastYear:dd/MM/yyyy} - {endDateSamePeriodLastYear:dd/MM/yyyy}] and [{toDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}]"
            };
        }

        public async Task<BookingStatusStats> GetBookingStatusStatsAsync(long facilityId, int year, CancellationToken cancellationToken)
        {
            ISpecification<Booking> spec = new BookingByYearEqualsSpecification(year);

            var result = new BookingStatusStats();
            var query = _dbContext.Bookings.AsNoTracking();

            if (_executionContext.IsSA)
            {
                if (facilityId > 0)
                {
                    spec = spec.And(new LimitByFacilityIdSpecification<Booking>(facilityId));
                }
            }
            else
            {
                if (facilityId > 0)
                {
                    spec = spec.And(new LimitByFacilityIdSpecification<Booking>(facilityId));
                }
                else
                {
                    spec = spec.And(new LimitByFacilityIdSpecification<Booking>(_executionContext.FacilityId));
                }
            }

            var bookings = await query.Where(spec.GetExpression())
                                      .Select(x => new Booking { Id = x.Id, Date = x.Date, Status = x.Status, FacilityId = x.FacilityId, FacilityNameVn = x.FacilityNameVn })
                                      .ToListAsync(cancellationToken);
            if (!bookings.Any())
            {
                return result;
            }

            var groups = bookings.GroupBy(x => x.Status);

            foreach (var group in groups)
            {
                var bookingsByGroup = group.ToList();

                result.Values.Add(new BookingStatusStatsValue
                {
                    StatusId = (int)group.Key,
                    StatusName = bookingsByGroup[0].Status.GetDescription(),
                    StatusNameEn = bookingsByGroup[0].Status.ToString(),
                    TotalCount = bookingsByGroup.Count,
                });
            }

            return result;
        }

        public async Task<TopFacility> GetTopFacilityPerYear(int year, CancellationToken cancellationToken)
        {
            ISpecification<Booking> spec = new BookingByYearEqualsSpecification(year);

            var result = new TopFacility();
            var query = _dbContext.Bookings.AsNoTracking();

            var facilities = await query
                        .Where(spec.GetExpression())
                        .GroupBy(b => new { b.FacilityId, b.FacilityNameVn, b.FacilityNameEn })
                        .Select(g => new TopFacilityValue
                        {
                            FacilityId = g.Key.FacilityId,
                            FacilityNameVn = g.Key.FacilityNameVn,
                            FacilityNameEn = g.Key.FacilityNameEn,
                            TotalCount = g.Count()
                        })
                        .OrderByDescending(x => x.TotalCount)
                        .Take(5)
                        .ToListAsync(cancellationToken);


            if (!facilities.Any())
            {
                return result;
            }

            result.Values = facilities;

            return result;
        }
    }
}
