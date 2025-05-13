using AutoMapper;
using Hospital.Application.Dtos.Zones;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Application.Repositories.Interfaces.Zones;
using Hospital.Domain.Entities.Zones;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MassTransit.Initializers;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Zones
{
    public class AddZoneCommandHandler : BaseCommandHandler, IRequestHandler<AddZoneCommand, string>
    {
        private readonly IZoneReadRepository _zoneReadRepository;
        private readonly IZoneWriteRepository _zoneWriteRepository;
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        private readonly IExecutionContext _executionContext;
        public AddZoneCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IZoneReadRepository zoneReadRepository,
            IZoneWriteRepository zoneWriteRepository,
            IHealthFacilityReadRepository healthFacilityReadRepository,
            ISpecialtyReadRepository specialtyReadRepository,
            IExecutionContext executionContext
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _zoneReadRepository = zoneReadRepository;
            _zoneWriteRepository = zoneWriteRepository;
            _healthFacilityReadRepository = healthFacilityReadRepository;
            _specialtyReadRepository = specialtyReadRepository;
            _executionContext = executionContext;
        }

        public async Task<string> Handle(AddZoneCommand request, CancellationToken cancellationToken)
        {
            var facility = await _healthFacilityReadRepository.GetByIdAsync(_executionContext.FacilityId, cancellationToken: cancellationToken);

            var specialties = await _specialtyReadRepository.GetAsync(cancellationToken: cancellationToken);

            if (facility == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            await ValidateAndThrowAsync(request.Zone, cancellationToken);

            var zone = _mapper.Map<Zone>(request.Zone);

            zone.FacilityId = _executionContext.FacilityId;

            zone.ZoneSpecialties = new();

            foreach (var specialtyId in request.Zone.SpecialtyIds)
            {
                var speDb = specialties.First(x => x.Id + "" == specialtyId);

                zone.ZoneSpecialties.Add(new ZoneSpecialty
                {
                    Id = AuthUtility.GenerateSnowflakeId(),
                    SpecialtyId = speDb.Id,
                });
            }

            await _zoneWriteRepository.AddAsync(zone, cancellationToken);

            return zone.Id.ToString();
        }

        public async Task ValidateAndThrowAsync(ZoneDto zone, CancellationToken cancellationToken)
        {
            var option = new QueryOption
            {
                Includes = new string[] { nameof(Zone.ZoneSpecialties) }
            };

            var zones = await _zoneReadRepository.GetAsync(option: option, cancellationToken: cancellationToken);

            foreach (var z in zones)
            {
                if (z.NameVn == zone.NameVn || z.NameEn == zone.NameEn)
                {
                    throw new BadRequestException(_localizer["Zone đã tồn tại"]);
                }

                var specialtyIds = z.ZoneSpecialties.Select(x => x.SpecialtyId.ToString()).ToList();

                if (zone.Specialties.Any(s => specialtyIds.Contains(s.Id)))
                {
                    throw new BadRequestException(_localizer["Specialty đã tồn tại trong một Zone khác"]);
                }

            }
        }
    }
}
