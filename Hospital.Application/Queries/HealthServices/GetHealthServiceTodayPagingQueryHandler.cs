using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetHealthServiceTodayPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetHealthServiceTodayPagingQuery, PaginationResult<HealthServiceDto>>
    {
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly IServiceTypeReadRepository _serviceTypeReadRepository;
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        public GetHealthServiceTodayPagingQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthServiceReadRepository healthServiceReadRepository,
            IServiceTypeReadRepository serviceTypeReadRepository,
            IHealthFacilityReadRepository healthFacilityReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthServiceReadRepository = healthServiceReadRepository;
            _serviceTypeReadRepository = serviceTypeReadRepository;
            _healthFacilityReadRepository = healthFacilityReadRepository;
        }

        public async Task<PaginationResult<HealthServiceDto>> Handle(GetHealthServiceTodayPagingQuery request, CancellationToken cancellationToken)
        {
            var result = await _healthServiceReadRepository.GetTodayPagingWithFilterAsync(request.Pagination, request.Status, request.TypeId, request.FacilityId, request.SpecialtyId, request.DoctorId, cancellationToken: cancellationToken);

            var types = await _serviceTypeReadRepository.GetAsync(cancellationToken: cancellationToken);

            List<HealthServiceDto> dtos = new();

            foreach (var entity in result.Data)
            {
                var facility = await _healthFacilityReadRepository.GetByIdAsync(entity.FacilityId, cancellationToken: cancellationToken);
                var dto = _mapper.Map<HealthServiceDto>(entity);
                var type = types.First(x => x.Id.ToString() == dto.TypeId);
                dto.TypeNameVn = type.NameVn;
                dto.TypeNameEn = type.NameEn;
                dto.Days = entity.ServiceTimeRules.Select(x => x.DayOfWeek.ToString()).ToList();
                dto.FacilityNameVn = facility.NameVn;
                dto.FacilityNameEn = facility.NameEn;
                dtos.Add(dto);
            }
            return new PaginationResult<HealthServiceDto>(dtos, result.Total);

        }
    }
}
