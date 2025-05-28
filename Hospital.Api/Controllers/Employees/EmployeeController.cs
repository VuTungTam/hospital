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
        public IActionResult GetEnums(string noneOption, bool vn) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<AccountStatus>(noneOption, vn) });

        [HttpGet("sequence/{admin}")]
        public async Task<IActionResult> GetSequence(bool admin, CancellationToken cancellationToken = default)
        {
            var table = new Employee().GetTableName();
            if (admin)
            {
                table = "admin";
            }
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

        [HttpGet]
        public async Task<IActionResult> GetEmployeePagination(
            int page, int size, int state, long zoneId, long roleId, long facilityId,
            string search = "", string asc = "", string desc = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetEmployeesPaginationQuery(pagination, (AccountStatus)state, zoneId, roleId, facilityId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPost]
        public async Task<IActionResult> Add(EmployeeDto employee, CancellationToken cancellationToken = default)
        {
            var command = new AddEmployeeCommand(employee);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPost("admin")]
        public async Task<IActionResult> Add(AdminDto admin, CancellationToken cancellationToken = default)
        {
            var command = new AddFacilityAdminCommand(admin);
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
