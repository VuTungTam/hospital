﻿using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Locations
{
    public class GetDistrictIdByNameQueryHandler : BaseQueryHandler, IRequestHandler<GetDistrictIdByNameQuery, int>
    {
        private readonly ILocationReadRepository _locationReadRepository;
        public GetDistrictIdByNameQueryHandler(
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ILocationReadRepository locationReadRepository
            ) : base(mapper, localizer)
        {
            _locationReadRepository = locationReadRepository;
        }

        public async Task<int> Handle(GetDistrictIdByNameQuery request, CancellationToken cancellationToken)
        {
            return await _locationReadRepository.GetDidByNameAsync(request.DName,request.Pid ,cancellationToken);
        }
    }
}
