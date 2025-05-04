using Hospital.Application.Commands.Bookings;
using Hospital.Application.Dtos.Bookings;
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
        public IActionResult GetEnums(string noneOption) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<BookingStatus>(noneOption) });

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
        public async Task<IActionResult> GetPaging(
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

        [HttpPut]
        public virtual async Task<IActionResult> Update(BookingDto dto, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateBookingCommand(dto), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpGet("current/{serviceId}")]
        public virtual async Task<IActionResult> GetCurrent(long serviceId, long timeSlotId, CancellationToken cancellationToken = default)
        {
            var query = new GetCurrentOrderQuery(serviceId, timeSlotId);

            var index = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = index });
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

        [HttpPut("cancel/{id}")]
        public virtual async Task<IActionResult> Cancel(long id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new CancelBookingCommand(id), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            var command = new DeleteBookingsCommand(ids);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse());
        }

        //[HttpGet("booked-slots")]
        //public virtual async Task<IActionResult> GetQuantity
        //    (long serviceId, 
        //    DateTime date, 
        //    TimeSpan serviceStartTime, 
        //    TimeSpan serviceEndTime, 

        //    CancellationToken cancellationToken = default)
        //{
        //    var query = new GetCurrentOrderQuery(serviceId, serviceStartTime, serviceEndTime);

        //    var index = await _mediator.Send(query, cancellationToken);

        //    return Ok(new SimpleDataResult { Data = index });
        //}
    }
}
