using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Entities.Bookings;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
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
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var service = await _healthServiceReadRepository.GetByIdAsync(request.Id,_healthServiceReadRepository.DefaultQueryOption, cancellationToken : cancellationToken);

            if (service == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var serviceDto = _mapper.Map<HealthServiceDto>(service);

            return serviceDto;
        }
    }
}
