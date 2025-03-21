using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Employees
{
    public class CheckEmployeePermissionIsCustomizeQueryHandler : BaseQueryHandler, IRequestHandler<CheckEmployeePermissionIsCustomizeQuery, bool>
    {
        private readonly IEmployeeReadRepository _employeeReadRepository;

        public CheckEmployeePermissionIsCustomizeQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IEmployeeReadRepository employeeReadRepository
        ) : base(authService, mapper, localizer)
        {
            _employeeReadRepository = employeeReadRepository;
        }

        public Task<bool> Handle(CheckEmployeePermissionIsCustomizeQuery request, CancellationToken cancellationToken)
        {
            if (request.EmployeeId <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            return _employeeReadRepository.IsEmployeeCustomizePermissionAsync(request.EmployeeId, cancellationToken);
        }
    }
}
