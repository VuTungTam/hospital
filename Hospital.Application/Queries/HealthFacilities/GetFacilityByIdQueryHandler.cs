using AutoMapper;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetFacilityByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetFacilityByIdQuery, HealthFacilityDto>
    {
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        public GetFacilityByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthFacilityReadRepository healthFacilityReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthFacilityReadRepository = healthFacilityReadRepository;
        }

        public async Task<HealthFacilityDto> Handle(GetFacilityByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }
            var entity = await _healthFacilityReadRepository.GetByIdAsync(request.Id, ignoreOwner: true, cancellationToken: cancellationToken);
            var facility = _mapper.Map<HealthFacilityDto>(entity);
            return facility;
        }
    }
}
