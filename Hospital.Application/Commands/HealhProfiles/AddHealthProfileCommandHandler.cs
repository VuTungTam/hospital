using AutoMapper;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealthProfiles
{
    public class AddHealthProfileCommandHandler : BaseCommandHandler, IRequestHandler<AddHealthProfileCommand, long>
    {
        public readonly IHealthProfileWriteRepository _HealthProfileWriteRepository;
        public readonly ILocationReadRepository _locationReadRepository;
        public AddHealthProfileCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IHealthProfileWriteRepository HealthProfileWriteRepository,
            ILocationReadRepository locationReadRepository,
            IMapper mapper
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _HealthProfileWriteRepository = HealthProfileWriteRepository;
            _locationReadRepository = locationReadRepository;
        }

        public async Task<long> Handle(AddHealthProfileCommand request, CancellationToken cancellationToken)
        {
            var healthProfile = _mapper.Map<HealthProfile>(request.Dto);
            healthProfile.Pname = await _locationReadRepository.GetNameByIdAsync(healthProfile.Pid, "province", cancellationToken);
            healthProfile.Dname = await _locationReadRepository.GetNameByIdAsync(healthProfile.Did, "district", cancellationToken);
            healthProfile.Wname = await _locationReadRepository.GetNameByIdAsync(healthProfile.Wid, "ward", cancellationToken);
            if (healthProfile.OwnerId == 0) { 

            }
            
            await _HealthProfileWriteRepository.AddAsync(healthProfile, cancellationToken);
            long HealthProfileId = healthProfile.Id;
            return HealthProfileId;
        }
    }
}
