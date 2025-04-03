using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Dtos.Zones;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Zones;
using Hospital.Domain.Entities.Zones;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Zones
{
    public class GetZonesPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetZonesPaginationQuery, PaginationResult<ZoneDto>>
    {
        private readonly IZoneReadRepository _zoneReadRepository;
        public GetZonesPaginationQueryHandler(
            IAuthService authService, 
            IMapper mapper, 
            IStringLocalizer<Resources> localizer,
            IZoneReadRepository zoneReadRepository
            ) : base(authService, mapper, localizer)
        {
            _zoneReadRepository = zoneReadRepository;
        }

        public async Task<PaginationResult<ZoneDto>> Handle(GetZonesPaginationQuery request, CancellationToken cancellationToken)
        {
            var option = new QueryOption
            {
                Includes = new string[] { nameof(Zone.ZoneSpecialties) }
            };

            var result = await _zoneReadRepository.GetPaginationAsync(request.Pagination, null, option: option, cancellationToken: cancellationToken);

            var dtos = _mapper.Map<List<ZoneDto>>(result.Data);

            return new PaginationResult<ZoneDto>(dtos, result.Total);
        }
    }
}
