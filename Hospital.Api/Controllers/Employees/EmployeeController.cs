using Hospital.Application.Commands.Employees;
using Hospital.Application.Dtos.Employee;
using Hospital.Application.Models;
using Hospital.Application.Queries.Employees;
using Hospital.Application.Queries.Sequences;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Employees
{
    public class EmployeeController : ApiBaseController
    {
        public EmployeeController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("filterable")]
        public IActionResult GetFilterable() => GetFilterable<Employee>();

        [HttpGet("enums"), AllowAnonymous]
        public IActionResult GetEnums(string noneOption) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<AccountStatus>(noneOption) });

        [HttpGet("sequence")]
        public async Task<IActionResult> GetSequence(CancellationToken cancellationToken = default)
        {
            var table = new Employee().GetTableName();
            return Ok(new SimpleDataResult { Data = await _mediator.Send(new GetSequenceQuery(table), cancellationToken) });
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetEmployeeByIdQuery(id);
            var employee = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = employee });
        }

        [HttpGet("include-actions/{id}")]
        public async Task<IActionResult> GetByIdIncludeActions(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetEmployeeByIdIncludeActionsQuery(id);
            var employee = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = employee });
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetIncludeActions(CancellationToken cancellationToken = default)
        {
            var query = new GetEmployeeThemselvesQuery();
            var employee = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = employee });
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetEmployeePagination(int page, int size, int state, long roleId, string search = "", string asc = "", string desc = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetEmployeesPaginationQuery(pagination, (AccountStatus)state, roleId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("is-permission-customized/{employeeId}")]
        public async Task<IActionResult> IsPermissionCustomized(long employeeId, CancellationToken cancellationToken = default)
        {
            var query = new CheckEmployeePermissionIsCustomizeQuery(employeeId);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpPost]
        public async Task<IActionResult> Add(EmployeeDto employee, CancellationToken cancellationToken = default)
        {
            var command = new AddEmployeeCommand(employee);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut("change-profile-for-employee")]
        public async Task<IActionResult> Update(EmployeeDto employee, CancellationToken cancellationToken = default)
        {
            var command = new UpdateEmployeeCommand(employee);
            await _mediator.Send(command, cancellationToken);

            return Ok(new BaseResponse());
        }

        [HttpPut("change-password-for-employee")]
        public async Task<IActionResult> ChangePasswordForEmployee(UserPwdModel model, CancellationToken cancellationToken = default)
        {
            var command = new UpdateEmployeePasswordCommand(model);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("set-as-default/{employeeId}")]
        public async Task<IActionResult> SetActionAsDefault(long employeeId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new SetEmployeeActionAsDefaultCommand(employeeId), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("update-additional-actions/{employeeId}")]
        public async Task<IActionResult> UpdateAdditionalActionAsDefault(long employeeId, List<AdditionalAction> actions, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new SetEmployeeAdditionalActionsCommand(employeeId, actions), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("update-status/{id}/{status}")]
        public async Task<IActionResult> UpdateState(long id, AccountStatus status, CancellationToken cancellationToken = default)
        {
            var command = new UpdateEmployeeStatusCommand(id, status);
            await _mediator.Send(command, cancellationToken);

            return Ok(new BaseResponse());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteEmployeesCommand(ids), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
