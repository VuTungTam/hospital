﻿using AutoMapper;
using Hospital.Application.Dtos.Locations;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Locations
{
    public class GetWardsPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetWardsPagingQuery, PagingResult<WardDto>>
    {
        private readonly ILocationReadRepository _locationReadRepository;
        public GetWardsPagingQueryHandler(
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ILocationReadRepository locationReadRepository
        ) : base(mapper, localizer)
        {
            _locationReadRepository = locationReadRepository;
        }

        public async Task<PagingResult<WardDto>> Handle(GetWardsPagingQuery request, CancellationToken cancellationToken)
        {
            var paging = await _locationReadRepository.GetWardsPagingAsync(request.DistrictId, request.Pagination, cancellationToken);
            var wards = _mapper.Map<List<WardDto>>(paging.Data);

            return new PagingResult<WardDto>(wards, paging.Total);
        }
    }
}
