using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Locations
{
    public class GetWardIdByNameQueryHandler : BaseQueryHandler, IRequestHandler<GetWardIdByNameQuery, int>
    {
        private readonly ILocationReadRepository _locationReadRepository;
        public GetWardIdByNameQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ILocationReadRepository locationReadRepository
        ) : base(authService, mapper, localizer)
        {
            _locationReadRepository = locationReadRepository;
        }

        public async Task<int> Handle(GetWardIdByNameQuery request, CancellationToken cancellationToken)
        {
            return await _locationReadRepository.GetWidByNameAsync(request.WName, request.Did, cancellationToken);
        }
    }
}
