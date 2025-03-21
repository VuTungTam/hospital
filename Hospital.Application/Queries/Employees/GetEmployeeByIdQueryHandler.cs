using AutoMapper;
using Hospital.Application.Dtos.Employee;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Employees
{
    public class GetEmployeeByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        private readonly IEmployeeReadRepository _employeeReadRepository;

        public GetEmployeeByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IEmployeeReadRepository employeeReadRepository
        ) : base(authService, mapper, localizer)
        {
            _employeeReadRepository = employeeReadRepository;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
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

            return _mapper.Map<EmployeeDto>(employee);
        }
    }
}
