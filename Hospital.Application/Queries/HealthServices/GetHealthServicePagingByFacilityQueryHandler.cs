using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetHealthServicePagingByFacilityQueryHandler : BaseQueryHandler, IRequestHandler<GetHealthServicePagingByFacilityQuery, PagingResult<HealthServiceDto>>
    {
        private readonly IHealthServiceReadRepository _readRepository;
        public GetHealthServicePagingByFacilityQueryHandler(
            IHealthServiceReadRepository readRepository,
            IMapper mapper,
            IStringLocalizer<Resources> localizer) :
            base(mapper, localizer)
        {
            _readRepository = readRepository;
        }

        public async Task<PagingResult<HealthServiceDto>> Handle(GetHealthServicePagingByFacilityQuery request, CancellationToken cancellationToken)
        {
            var paging = await _readRepository.GetPagingByFacilityAsync(request.Pagination, request.FacilityId, null, cancellationToken);
            var services = _mapper.Map<List<HealthServiceDto>>(paging.Data);

            return new PagingResult<HealthServiceDto>(services, paging.Total);
        }
    }
}
