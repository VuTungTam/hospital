using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetServiceTypeQueryHandler : BaseQueryHandler, IRequestHandler<GetServiceTypeQuery, List<ServiceTypeDto>>
    {
        private readonly IServiceTypeReadRepository _serviceTypeReadRepository;

        public GetServiceTypeQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IServiceTypeReadRepository serviceTypeReadRepositor
        ) : base(authService, mapper, localizer)
        {
            _serviceTypeReadRepository = serviceTypeReadRepositor;
        }

        public async Task<List<ServiceTypeDto>> Handle(GetServiceTypeQuery request, CancellationToken cancellationToken)
        {
            var types = await _serviceTypeReadRepository.GetAsync(cancellationToken: cancellationToken);

            return _mapper.Map<List<ServiceTypeDto>>(types);
        }
    }
}
