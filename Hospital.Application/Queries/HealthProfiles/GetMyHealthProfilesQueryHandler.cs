using AutoMapper;
using Hospital.Application.Dtos.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthProfiles
{
    public class GetMyHealthProfilesQueryHandler : BaseQueryHandler, IRequestHandler<GetMyHealthProfilesQuery, List<HealthProfileDto>>
    {
        private readonly IHealthProfileReadRepository _healthProfileReadRepository;
        public GetMyHealthProfilesQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthProfileReadRepository healthProfileReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthProfileReadRepository = healthProfileReadRepository;
        }

        public async Task<List<HealthProfileDto>> Handle(GetMyHealthProfilesQuery request, CancellationToken cancellationToken)
        {
            var option = new QueryOption
            {
                IgnoreOwner = false,
            };

            var profiles = await _healthProfileReadRepository.GetAsync(null, option, cancellationToken);

            var profileDtos = _mapper.Map<List<HealthProfileDto>>(profiles);

            return profileDtos;
        }
    }
}
