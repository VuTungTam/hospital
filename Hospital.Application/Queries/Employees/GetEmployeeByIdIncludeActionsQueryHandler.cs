using AutoMapper;
using Hospital.Application.Dtos.Auth;
using Hospital.Application.Dtos.Employee;
using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Employees
{
    public class GetEmployeeByIdIncludeActionsQueryHandler : BaseQueryHandler, IRequestHandler<GetEmployeeByIdIncludeActionsQuery, EmployeeDto>
    {
        private readonly IActionReadRepository _actionReadRepository;
        private readonly IEmployeeReadRepository _employeeReadRepository;

        public GetEmployeeByIdIncludeActionsQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IActionReadRepository actionReadRepository,
            IEmployeeReadRepository employeeReadRepository
        ) : base(authService, mapper, localizer)
        {
            _actionReadRepository = actionReadRepository;
            _employeeReadRepository = employeeReadRepository;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByIdIncludeActionsQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var employee = await _employeeReadRepository.GetByIdIncludedRolesAsync(request.Id, cancellationToken);
            if (employee == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var dto = _mapper.Map<EmployeeDto>(employee);
            var actions = await _actionReadRepository.GetActionsByEmployeeIdAsync(request.Id, cancellationToken);

            dto.Actions = _mapper.Map<List<ActionDto>>(actions);

            return dto;
        }
    }
}
