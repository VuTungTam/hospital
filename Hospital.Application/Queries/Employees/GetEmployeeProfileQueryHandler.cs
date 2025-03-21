using AutoMapper;
using Hospital.Application.Dtos.Auth;
using Hospital.Application.Dtos.Employee;
using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;


namespace Hospital.Application.Queries.Employees
{
    public class GetEmployeeProfileQueryHandler : BaseQueryHandler, IRequestHandler<GetEmployeeProfileQuery, EmployeeDto>
    {
        private readonly IActionReadRepository _actionReadRepository;
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly IExecutionContext _executionContext;

        public GetEmployeeProfileQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IActionReadRepository actionReadRepository,
            IEmployeeReadRepository employeeReadRepository,
            IExecutionContext executionContext
        ) : base(authService, mapper, localizer)
        {
            _actionReadRepository = actionReadRepository;
            _employeeReadRepository = employeeReadRepository;
            _executionContext = executionContext;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeProfileQuery request, CancellationToken cancellationToken)
        {
            Employee employee;
            if (request.IncludeRole)
            {
                employee = await _employeeReadRepository.GetByIdIncludedRolesAsync(_executionContext.Identity, cancellationToken);
            }
            else
            {
                employee = await _employeeReadRepository.GetByIdAsync(_executionContext.Identity, cancellationToken: cancellationToken);
            }

            if (employee == null)
            {
                return null;
            }

            var dto = _mapper.Map<EmployeeDto>(employee);

            if (request.IncludeRole)
            {
                var actions = await _actionReadRepository.GetActionsByEmployeeIdAsync(employee.Id, cancellationToken);
                dto.Actions = _mapper.Map<List<ActionDto>>(actions);
            }

            return dto;
        }
    }
}
