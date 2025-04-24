using AutoMapper;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
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

            var option = new QueryOption
            {
                Includes = new string[] { nameof(HealthFacility.Images), nameof(HealthFacility.FacilitySpecialties), nameof(HealthFacility.FacilityTypeMappings) }
            };

            var facility = await _healthFacilitReadRepository.GetByIdAsync(id, option, cancellationToken: cancellationToken);

            if (facility == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            await _healthFacilityWriteRepository.UpdateFacilityAsync(facility, request.HealthFacility, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
