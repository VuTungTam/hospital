using AutoMapper;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetAllFacilityQueryHandler : BaseQueryHandler, IRequestHandler<GetAllFacilityQuery, List<HealthFacilityDto>>
    {
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;

        public GetAllFacilityQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthFacilityReadRepository healthFacilityReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthFacilityReadRepository = healthFacilityReadRepository;
        }

        public async Task<List<HealthFacilityDto>> Handle(GetAllFacilityQuery request, CancellationToken cancellationToken)
        {
            var result = await _healthFacilityReadRepository.GetAsync(cancellationToken: cancellationToken);

            var facilities = _mapper.Map<List<HealthFacilityDto>>(result);

            return facilities;
        }
    }
}
