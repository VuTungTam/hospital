using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetServiceTypeByFacilityIdQueryHandler : BaseQueryHandler, IRequestHandler<GetServiceTypeByFacilityIdQuery, List<ServiceTypeDto>>
    {
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;

        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        public GetServiceTypeByFacilityIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthServiceReadRepository healthServiceReadRepository,
            IHealthFacilityReadRepository healthFacilityReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthServiceReadRepository = healthServiceReadRepository;
            _healthFacilityReadRepository = healthFacilityReadRepository;
        }

        public async Task<List<ServiceTypeDto>> Handle(GetServiceTypeByFacilityIdQuery request, CancellationToken cancellationToken)
        {
            if (request.FacilityId <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var facility = await _healthFacilityReadRepository.GetByIdAsync(request.FacilityId, cancellationToken: cancellationToken);

            if (facility == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var serviceTypes = await _healthServiceReadRepository.GetServiceTypeByFacilityIdAsync(request.FacilityId, cancellationToken: cancellationToken);

            var serviceTypeDtos = _mapper.Map<List<ServiceTypeDto>>(serviceTypes);

            return serviceTypeDtos;
        }
    }
}
