using AutoMapper;
using Hospital.Application.Dtos.Locations;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Locations
{
    public class GetProvincesPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetProvincesPagingQuery, PagingResult<ProvinceDto>>
    {
        private readonly ILocationReadRepository _locationReadRepository;

        public GetProvincesPagingQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ILocationReadRepository locationReadRepository
        ) : base(authService, mapper, localizer)
        {
            _locationReadRepository = locationReadRepository;
        }

        public async Task<PagingResult<ProvinceDto>> Handle(GetProvincesPagingQuery request, CancellationToken cancellationToken)
        {
            var paging = await _locationReadRepository.GetProvincesPagingAsync(request.Pagination, cancellationToken);
            var provinces = _mapper.Map<List<ProvinceDto>>(paging.Data);
            return new PagingResult<ProvinceDto> ( provinces, paging.Total );
        }
    }
}
