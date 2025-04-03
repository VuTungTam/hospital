using Hospital.Application.Commands.Customers;
using Hospital.Application.Dtos.Customers;
using Hospital.Application.Models;
using Hospital.Application.Queries.Customers;
using Hospital.Application.Queries.Sequences;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Customers
{
    public class CustomerController : ApiBaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerController(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor
        ) : base(mediator)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("filterable")]
        public IActionResult GetFilterable() => GetFilterable<Customer>();


        [HttpGet("enums"), AllowAnonymous]
        public IActionResult GetEnums(string noneOption) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<AccountStatus>(noneOption) });

        [HttpGet("sequence")]
        public async Task<IActionResult> GetCustomerSequence(CancellationToken cancellationToken = default)
        {
            var table = new Customer().GetTableName();
            return Ok(new SimpleDataResult { Data = await _mediator.Send(new GetSequenceQuery(table), cancellationToken) });
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetCustomerByIdQuery(id);
            var customer = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = customer });
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination(int page, int size, int state, string search = "", string asc = "", string desc = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetCustomersPaginationQuery(pagination, (AccountStatus)state);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("names")]
        public async Task<IActionResult> GetNames(CancellationToken cancellationToken = default)
        {
            var customerNames = await _mediator.Send(new GetCustomerNamesQuery(), cancellationToken);
            return Ok(new ServiceResult { Data = customerNames, Total = customerNames.Count });
        }

        [HttpPost]
        public async Task<IActionResult> Add(CustomerDto customer, CancellationToken cancellationToken = default)
        {
            var command = new AddCustomerCommand(customer);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut("change-profile-for-customer")]
        public async Task<IActionResult> Update(CustomerDto customer, CancellationToken cancellationToken = default)
        {
            var command = new UpdateCustomerCommand(customer);
            await _mediator.Send(command, cancellationToken);

            return Ok(new BaseResponse());
        }

        [HttpPut("change-password-for-customer")]
        public async Task<IActionResult> ChangePasswordForCustomer(UserPwdModel model, CancellationToken cancellationToken = default)
        {
            var command = new UpdateCustomerPasswordCommand(model);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("update-status/{id}/{status}")]
        public async Task<IActionResult> UpdateState(long id, AccountStatus status, CancellationToken cancellationToken = default)
        {
            var command = new UpdateCustomerStatusCommand(id, status);
            await _mediator.Send(command, cancellationToken);

            return Ok(new BaseResponse());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteCustomersCommand(ids), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
