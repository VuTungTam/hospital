using AutoMapper;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetHealthFacilityPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetHealthFacilityPaginationQuery, PaginationResult<HealthFacilityDto>>
    {
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        private readonly IFacilityTypeReadRepository _facilityTypeReadRepository;

        public GetHealthFacilityPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthFacilityReadRepository healthFacilityReadRepository,
            IFacilityTypeReadRepository facilityTypeReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthFacilityReadRepository = healthFacilityReadRepository;
            _facilityTypeReadRepository = facilityTypeReadRepository;
        }

        public async Task<PaginationResult<HealthFacilityDto>> Handle(GetHealthFacilityPaginationQuery request, CancellationToken cancellationToken)
        {
            var facilities = await _healthFacilityReadRepository.GetPagingWithFilterAsync(request.Pagination, request.TypeId, request.ServiceTypeId, request.Status, cancellationToken);
            List<HealthFacilityDto> facilitieDtos = new();
            foreach (var facility in facilities.Data)
            {
                var facilityDto = _mapper.Map<HealthFacilityDto>(facility);

                var typeIds = facility.FacilityTypeMappings.Select(ds => ds.TypeId).ToList();

                var types = await _facilityTypeReadRepository.GetByIdsAsync(typeIds, cancellationToken: cancellationToken);

                var typeDtos = _mapper.Map<List<FacilityTypeDto>>(types);

                facilityDto.Types = typeDtos;
                facilitieDtos.Add(facilityDto);
            }

            return new PaginationResult<HealthFacilityDto>(facilitieDtos, facilities.Total);
        }
    }
}
