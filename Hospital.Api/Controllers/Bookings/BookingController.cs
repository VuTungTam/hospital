using Hospital.Application.Commands.Bookings;
using Hospital.Application.Dtos.Bookings;
using Hospital.Application.Models.Bookings;
using Hospital.Application.Queries.Bookings;
using Hospital.Application.Queries.Sequences;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Bookings
{
    public class BookingController : ApiBaseController
    {
        public BookingController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("filterable")]
        public IActionResult GetFilterable() => base.GetFilterable<Booking>();

        [HttpGet("enums"), AllowAnonymous]
        public IActionResult GetEnums(string noneOption, bool vn) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<BookingStatus>(noneOption, vn) });

        [HttpGet("sequence")]
        public async Task<IActionResult> GetSequence(CancellationToken cancellationToken = default)
        {
            var table = new Booking().GetTableName();
            return Ok(new SimpleDataResult { Data = await _mediator.Send(new GetSequenceQuery(table), cancellationToken) });
        }


        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetBookingByIdQuery(id);
            var user = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = user });
        }

        [HttpGet("code/{code}")]
        public virtual async Task<IActionResult> GetByCode(string code, CancellationToken cancellationToken = default)
        {
            var query = new GetBookingByCodeQuery(code);
            var user = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = user });
        }

        [HttpGet("count"), AllowAnonymous]
        public virtual async Task<IActionResult> GetCount(long timeRuleId, DateTime date, CancellationToken cancellationToken = default)
        {
            var query = new GetBookingCountQuery(timeRuleId, date);
            var user = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = user });
        }

        [HttpGet("myself")]
        public async Task<IActionResult> GetMyListPagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            long serviceId = 0,
            DateTime date = default,
            BookingStatus status = BookingStatus.None,
            CancellationToken cancellationToken = default
        )
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetMyListBookingsPagingQuery(pagination, status, serviceId, date);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet]
        public async Task<IActionResult> GetPagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            long owner = 0,
            DateTime date = default,
            BookingStatus status = BookingStatus.None,
            long excludeId = 0,
            CancellationToken cancellationToken = default
        )
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetBookingsPagingQuery(pagination, owner, status, date, excludeId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("doctor")]
        public async Task<IActionResult> GetDoctorPagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            long ownerId = 0,
            long serviceId = 0,
            long timeSlotId = 0,
            DateTime date = default,
            BookingStatus status = BookingStatus.None,
            CancellationToken cancellationToken = default
        )
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetDoctorBookingsPagingQuery(pagination, status, serviceId, timeSlotId, date, ownerId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("queue")]
        public async Task<IActionResult> GetQueuePagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            long serviceId = 0,
            long timeSlotId = 0,
            DateTime date = default,
            CancellationToken cancellationToken = default
        )
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetQueueBookingsPagingQuery(pagination, serviceId, timeSlotId, date);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPost("book-an-appointment"), AllowAnonymous]
        public virtual async Task<IActionResult> BookAnAppointment(BookingDto dto, CancellationToken cancellationToken = default)
        {
            var command = new BookAnAppointmentCommand(dto);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add(BookingDto dto, CancellationToken cancellationToken = default)
        {
            var command = new AddBookingCommand(dto);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPost("test-schedule"), AllowAnonymous]
        public virtual async Task<IActionResult> Test(CancellationToken cancellationToken = default)
        {
            var command = new TestSchedule();
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(BookingDto dto, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateBookingCommand(dto), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpGet("current/{serviceId}/{timeSlotId}")]
        public virtual async Task<IActionResult> GetCurrent(long serviceId, long timeSlotId, CancellationToken cancellationToken = default)
        {
            var query = new GetCurrentOrderQuery(serviceId, timeSlotId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpGet("booking-current/{bookingId}")]
        public virtual async Task<IActionResult> CheckCurrent(long bookingId, CancellationToken cancellationToken = default)
        {
            var query = new CheckCurrentBookingQuery(bookingId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpPut("confirm/{id}")]
        public virtual async Task<IActionResult> Confirm(long id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new ConfirmBookingCommand(id), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("reject/{id}")]
        public virtual async Task<IActionResult> Reject(long id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new RejectBookingCommand(id), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("cancel")]
        public virtual async Task<IActionResult> Cancel(CancelBookingModel model, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new CancelBookingCommand(model), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("start/{id}")]
        public virtual async Task<IActionResult> Start(long id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new StartBookingCommand(id), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("complete/{id}")]
        public virtual async Task<IActionResult> Complete(long id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new CompleteBookingCommand(id), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            var command = new DeleteBookingsCommand(ids);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
