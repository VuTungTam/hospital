using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Dtos.Locations;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetHealthServicePagingByTypeQueryHandler : BaseQueryHandler, IRequestHandler<GetHealthServicePagingByTypeQuery,PagingResult<HealthServiceDto>>
    {
        private readonly IHealthServiceReadRepository _readRepository;
        public GetHealthServicePagingByTypeQueryHandler(
            IHealthServiceReadRepository readRepository,
            IMapper mapper, 
            IStringLocalizer<Resources> localizer) : 
            base(mapper, localizer)
        {
            _readRepository = readRepository;
        }
        public async Task<PagingResult<HealthServiceDto>> Handle(GetHealthServicePagingByTypeQuery request, CancellationToken cancellationToken)
        {
            var paging = await _readRepository.GetPagingByTypeAsync(request.Pagination, request.TypeId, null, cancellationToken);
            var services = _mapper.Map<List<HealthServiceDto>>(paging.Data);

            return new PagingResult<HealthServiceDto>(services, paging.Total);
        }
    }
}
