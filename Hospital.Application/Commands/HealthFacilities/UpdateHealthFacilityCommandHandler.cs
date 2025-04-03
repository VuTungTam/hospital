using AutoMapper;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealthFacilities
{
    public class UpdateHealthFacilityCommandHandler : BaseCommandHandler, IRequestHandler<UpdateHealthFacilityCommand>
    {
        private readonly IHealthFacilityWriteRepository _healthFacilityWriteRepository;
        private readonly IHealthFacilityReadRepository _healthFacilitReadRepository;
        public UpdateHealthFacilityCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IHealthFacilityWriteRepository healthFacilityWriteRepository,
            IHealthFacilityReadRepository healthFacilitReadRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _healthFacilityWriteRepository = healthFacilityWriteRepository;
            _healthFacilitReadRepository = healthFacilitReadRepository;
        }

        public async Task<Unit> Handle(UpdateHealthFacilityCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.HealthFacility.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var facility = _healthFacilitReadRepository.GetByIdAsync(id, _healthFacilitReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);

            if (facility == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }
            var entity = _mapper.Map<HealthFacility>(request.HealthFacility);

            await _healthFacilityWriteRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
