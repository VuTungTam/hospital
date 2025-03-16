using AutoMapper;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealthFacilities
{
    public class AddHealthFacilityCommandHandler : BaseCommandHandler, IRequestHandler<AddHealthFacilityCommand, string>
    {
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        private readonly IHealthFacilityWriteRepository _healthFacilityWriteRepository;

        public AddHealthFacilityCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IHealthFacilityReadRepository healthFacilityReadRepository,
            IHealthFacilityWriteRepository healthFacilityWriteRepository,
            IMapper mapper
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _healthFacilityReadRepository = healthFacilityReadRepository;   
            _healthFacilityWriteRepository = healthFacilityWriteRepository;
        }

        public async Task<string> Handle(AddHealthFacilityCommand request, CancellationToken cancellationToken)
        {
            var facility = _mapper.Map<HealthFacility>(request.HealthFacility);

            await _healthFacilityWriteRepository.AddAsync(facility, cancellationToken);

            return facility.Id.ToString();
        }
    }
}
