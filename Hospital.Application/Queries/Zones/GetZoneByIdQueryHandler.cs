using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Dtos.Zones;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Zones;
using Hospital.Domain.Entities.Zones;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Zones
{
    public class GetZoneByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetZoneByIdQuery, ZoneDto>
    {
        private readonly IZoneReadRepository _zoneReadRepository;
        public GetZoneByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IZoneReadRepository zoneReadRepository,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _zoneReadRepository = zoneReadRepository;
        }

        public async Task<ZoneDto> Handle(GetZoneByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }
            var option = new QueryOption
            {
                Includes = new string[] { nameof(Zone.ZoneSpecialties) }
            };

            var result = await _zoneReadRepository.GetByIdAsync(request.Id, option: option, cancellationToken: cancellationToken);

            if (result == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var zone = _mapper.Map<ZoneDto>(result);

            return zone;
        }
    }
}
