using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Zones;
using Hospital.Domain.Entities.Zones;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Zones
{
    public class DeleteZoneCommandHandler : BaseCommandHandler, IRequestHandler<DeleteZoneCommand>
    {
        private readonly IZoneReadRepository _zoneReadRepository;
        private readonly IZoneWriteRepository _zoneWriteRepository;
        public DeleteZoneCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IZoneReadRepository zoneReadRepository,
            IZoneWriteRepository zoneWriteRepository,
            IMapper mapper
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _zoneReadRepository = zoneReadRepository;
            _zoneWriteRepository = zoneWriteRepository;
        }

        public async Task<Unit> Handle(DeleteZoneCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || !request.Ids.Any())
            {
                return Unit.Value;
            }

            QueryOption option = new QueryOption
            {
                Includes = new string[] { nameof(Zone.ZoneSpecialties) }
            };

            var zones = await _zoneReadRepository.GetByIdsAsync(request.Ids, option: option, cancellationToken: cancellationToken);

            foreach (var zone in zones)
            {
                await _zoneWriteRepository.RemoveSpecialtiesAsync(zone.ZoneSpecialties, cancellationToken);
            }

            _zoneWriteRepository.Delete(zones);

            await _zoneWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            await _zoneWriteRepository.RemoveCacheWhenDeleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
