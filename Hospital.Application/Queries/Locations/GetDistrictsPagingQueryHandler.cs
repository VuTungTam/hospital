using AutoMapper;
using Hospital.Application.Dtos.Locations;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Locations
{
    public class GetDistrictsPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetDistrictsPagingQuery, PagingResult<DistrictDto>>
    {
        private readonly ILocationReadRepository _locationReadRepository;

        public GetDistrictsPagingQueryHandler(
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ILocationReadRepository locationReadRepository
        ) : base(mapper, localizer)
        {
            _locationReadRepository = locationReadRepository;
        }

        public async Task<PagingResult<DistrictDto>> Handle(GetDistrictsPagingQuery request, CancellationToken cancellationToken)
        {
            var paging = await _locationReadRepository.GetDistrictsPagingAsync(request.ProvinceId, request.Pagination, cancellationToken);
            var districts = _mapper.Map<List<DistrictDto>>(paging.Data);

            return new PagingResult<DistrictDto>(districts, paging.Total);
        }
    }
}
