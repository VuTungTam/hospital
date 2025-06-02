using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Dtos.Zones;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Specialities;
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
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        public GetZoneByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IZoneReadRepository zoneReadRepository,
            ISpecialtyReadRepository specialtyReadRepository,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _zoneReadRepository = zoneReadRepository;
            _specialtyReadRepository = specialtyReadRepository;
        }

        public async Task<ZoneDto> Handle(GetZoneByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }
            var option = new QueryOption
            {
                Includes = new string[] { nameof(Zone.ZoneSpecialties) }
            };

            var zone = await _zoneReadRepository.GetByIdAsync(request.Id, option: option, cancellationToken: cancellationToken);

            if (zone == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }
            var speIds = zone.ZoneSpecialties.Select(ds => ds.SpecialtyId).ToList();

            var specialties = await _specialtyReadRepository.GetByIdsAsync(speIds, cancellationToken: cancellationToken);

            var specialtyDtos = _mapper.Map<List<SpecialtyDto>>(specialties);

            var zoneDto = _mapper.Map<ZoneDto>(zone);

            zoneDto.Specialties = specialtyDtos;

            return zoneDto;
        }
    }
}
