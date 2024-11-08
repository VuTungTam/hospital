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
    public class GetHealthServiceByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetHealthServiceByIdQuery, HealthServiceDto>
    {
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        public GetHealthServiceByIdQueryHandler(
            IAuthService authService, 
            IMapper mapper, 
            IStringLocalizer<Resources> localizer,
            IHealthServiceReadRepository healthServiceReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthServiceReadRepository = healthServiceReadRepository;
        }

        public async Task<HealthServiceDto> Handle(GetHealthServiceByIdQuery request, CancellationToken cancellationToken)
        {
            var service = await _healthServiceReadRepository.GetByIdAsync(request.Id, cancellationToken : cancellationToken);

            var result = _mapper.Map<HealthServiceDto>(service);

            return result;
        }
    }
}
