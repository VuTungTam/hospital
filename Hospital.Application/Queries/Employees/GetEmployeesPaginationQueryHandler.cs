using AutoMapper;
using Hospital.Application.Dtos.Employee;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Employees
{
    public class GetEmployeesPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetEmployeesPaginationQuery, PaginationResult<EmployeeDto>>
    {
        private readonly IEmployeeReadRepository _employeeReadRepository;

        public GetEmployeesPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IEmployeeReadRepository employeeReadRepository
        ) : base(authService, mapper, localizer)
        {
            _employeeReadRepository = employeeReadRepository;
        }

        public async Task<PaginationResult<EmployeeDto>> Handle(GetEmployeesPaginationQuery request, CancellationToken cancellationToken)
        {
            var pagination = await _employeeReadRepository.GetEmployeesPaginationResultAsync(request.Pagination, request.State, request.ZoneId, request.RoleId, cancellationToken);
            var dtos = _mapper.Map<List<EmployeeDto>>(pagination.Data);

            return new PaginationResult<EmployeeDto>(dtos, pagination.Total);
        }
    }
}
