using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Zones;
using Hospital.Domain.Entities.Zones;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Zones
{
    public class UpdateZoneCommandHandler : BaseCommandHandler, IRequestHandler<UpdateZoneCommand>
    {
        private readonly IZoneReadRepository _zoneReadRepository;
        private readonly IZoneWriteRepository _zoneWriteRepository;
        public UpdateZoneCommandHandler(
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

        public async Task<Unit> Handle(UpdateZoneCommand request, CancellationToken cancellationToken)
        {
            QueryOption option = new QueryOption
            {
                Includes = new string[] { nameof(Zone.ZoneSpecialties) }
            };

            var zone = await _zoneReadRepository.GetByIdAsync(long.Parse(request.Zone.Id), option, cancellationToken: cancellationToken);

            if (zone == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            await _zoneWriteRepository.UpdateZoneAsync(zone, request.Zone, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
