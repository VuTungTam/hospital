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
    public class GetFacilityBySlugQueryHandler : BaseQueryHandler, IRequestHandler<GetFacilityBySlugQuery, HealthFacilityDto>
    {
        private readonly IHealthFacilityReadRepository _facilityReadRepository;

        public GetFacilityBySlugQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthFacilityReadRepository facilityReadRepositor
        ) : base(authService, mapper, localizer)
        {
            _facilityReadRepository = facilityReadRepositor;
        }

        public async Task<HealthFacilityDto> Handle(GetFacilityBySlugQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Slug))
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var facility = await _facilityReadRepository.GetBySlugAndLangsAsync(request.Slug, request.Langs, cancellationToken);

            return _mapper.Map<HealthFacilityDto>(facility);
        }
    }
}
