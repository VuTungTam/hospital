using Hospital.Api.Controllers;
using Hospital.Application.Queries.Dashboards.Articles;
using Hospital.Application.Queries.Dashboards.Bookings;
using Hospital.Application.Queries.Dashboards.Customers;
using Hospital.Application.Queries.Dashboards.HealthServices;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Dashboards
{
    public class DashboardController : ApiBaseController
    {
        public DashboardController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("booking/{facilityId}/{isOnlyCompleted}/{year}")]
        public async Task<IActionResult> GetBookingDashboard(long facilityId, bool isOnlyCompleted, int year, CancellationToken cancellationToken = default)
        {
            var query = new GetBookingDashboardByYearQuery(year, facilityId, isOnlyCompleted);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpGet("yearly-facility-booking-stats/{isOnlyCompleted}/{year}")]
        public async Task<IActionResult> GetYearlyFacilityBookingStats(bool isOnlyCompleted, int year, CancellationToken cancellationToken = default)
        {
            var query = new GetYearlyFacilityBookingStatsQuery(year, isOnlyCompleted);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpGet("yearly-booking-status-stats/{facilityId}/{year}")]
        public async Task<IActionResult> GetYearlyBookingStatusStats(long facilityId, int year, CancellationToken cancellationToken = default)
        {
            var query = new GetBookingDashboardByStatusQuery(year, facilityId);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpGet("top-facilities-per-year/{year}")]
        public async Task<IActionResult> GetTopFacilityPerYear(int year, CancellationToken cancellationToken = default)
        {
            var query = new GetTopFacilityPerYearQuery(year);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpGet("yearly-service-stats/{facilityId}/{isOnlyCompleted}/{top}/{year}")]
        public async Task<IActionResult> GetYearlyServiceStats(long facilityId, bool isOnlyCompleted, int year, int top = 10, CancellationToken cancellationToken = default)
        {
            var query = new GetYearlyServiceStatsQuery(facilityId, year, top, isOnlyCompleted);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpGet("article-stats/{top}")]
        public async Task<IActionResult> GetArticleStats(int top = 3, CancellationToken cancellationToken = default)
        {
            var query = new GetArticleStatsQuery(top);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpGet("booking-trend/{facilityId}")]
        public async Task<IActionResult> GetBookingTrend(long facilityId, CancellationToken cancellationToken = default)
        {
            var query = new GetBookingDashboardTrendQuery(facilityId);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpGet("customer-trend")]
        public async Task<IActionResult> GetCustomerTrend(CancellationToken cancellationToken = default)
        {
            var query = new GetCustomerTrendQuery();
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }
    }
}
