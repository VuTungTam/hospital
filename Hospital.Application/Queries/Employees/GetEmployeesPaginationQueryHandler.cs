using AutoMapper;
using Hospital.Application.Dtos.Employee;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Employees
{
    public class GetEmployeesPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetEmployeesPaginationQuery, PaginationResult<EmployeeDto>>
    {
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly IExecutionContext _ececutionContext; 
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        public GetEmployeesPaginationQueryHandler(
            IExecutionContext excecutionContext,
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IEmployeeReadRepository employeeReadRepository,
            IHealthFacilityReadRepository healthFacilityReadRepository
        ) : base(authService, mapper, localizer)
        {
            _employeeReadRepository = employeeReadRepository;
            _ececutionContext = excecutionContext;
            _healthFacilityReadRepository = healthFacilityReadRepository;
        }

        public async Task<PaginationResult<EmployeeDto>> Handle(GetEmployeesPaginationQuery request, CancellationToken cancellationToken)
        {
            var pagination = await _employeeReadRepository.GetEmployeesPaginationResultAsync(request.Pagination, request.State, request.ZoneId, request.RoleId, request.FacilityId, cancellationToken);

            var dtos = _mapper.Map<List<EmployeeDto>>(pagination.Data);

            if (_ececutionContext.IsSA)
            {
                foreach (var dto in dtos)
                {
                    if (dto.FacilityId == "0")
                    {
                        dto.FacilityNameVn = "Hệ thống";

                        dto.FacilityNameEn = "System";
                    }
                    else
                    {
                        var facility = await _healthFacilityReadRepository.GetByIdAsync(long.Parse(dto.FacilityId), cancellationToken: cancellationToken);

                        dto.FacilityNameVn = facility.NameVn;

                        dto.FacilityNameEn = facility.NameEn;
                    }
                }
            }
            
            return new PaginationResult<EmployeeDto>(dtos, pagination.Total);
        }
    }
}
