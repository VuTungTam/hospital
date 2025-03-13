using AutoMapper;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetHealthFacilityPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetHealthFacilityPagingQuery, PagingResult<HealthFacilityDto>>
    {
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;   
        
        public GetHealthFacilityPagingQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthFacilityReadRepository healthFacilityReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthFacilityReadRepository = healthFacilityReadRepository;
        }

        public async Task<PagingResult<HealthFacilityDto>> Handle(GetHealthFacilityPagingQuery request, CancellationToken cancellationToken)
        {
            var facilities = await _healthFacilityReadRepository.GetPagingWithFilterAsync(request.Pagination, request.TypeId, request.Status, cancellationToken);
            
            var facilitieDtos = _mapper.Map<List< HealthFacilityDto>>(facilities.Data);
            
            return new PagingResult<HealthFacilityDto>(facilitieDtos, facilities.Total );
        }
    }
}
