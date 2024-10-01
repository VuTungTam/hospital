using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Locations
{
    public class GetProvinceIdByNameQueryHandler : BaseQueryHandler, IRequestHandler<GetProvinceIdByNameQuery, int>
    {
        private readonly ILocationReadRepository _locationReadRepository;

        public GetProvinceIdByNameQueryHandler(
            IMapper mapper, 
            IStringLocalizer<Resources> localizer,
            ILocationReadRepository locationReadRepository
            ) : base(mapper, localizer)
        {
            _locationReadRepository = locationReadRepository;
        }

        public async Task<int> Handle(GetProvinceIdByNameQuery request, CancellationToken cancellationToken)
        {
            return await _locationReadRepository.GetPidByNameAsync(request.PName, cancellationToken);
        }
    }
}
