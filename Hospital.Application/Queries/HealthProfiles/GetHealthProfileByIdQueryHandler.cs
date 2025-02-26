using AutoMapper;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Dtos.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthProfiles
{
    public class GetHealthProfileByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetHealthProfileByIdQuery, HealthProfileDto>
    {
        private readonly IHealthProfileReadRepository _healthProfileReadRepository;
        public GetHealthProfileByIdQueryHandler(
            IAuthService authService, 
            IMapper mapper,
            IHealthProfileReadRepository healthProfileReadRepository,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _healthProfileReadRepository = healthProfileReadRepository;
        }

        public async Task<HealthProfileDto> Handle(GetHealthProfileByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }
            var entity = await _healthProfileReadRepository.GetByIdAsync(request.Id, ignoreOwner: true, cancellationToken: cancellationToken);
            var profile = _mapper.Map<HealthProfileDto>(entity);
            return profile;
        }
    }
}
