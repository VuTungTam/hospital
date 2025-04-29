using AutoMapper;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetServiceTypeBySlugQueryHandler : BaseQueryHandler, IRequestHandler<GetServiceTypeBySlugQuery, ServiceTypeDto>
    {
        private readonly IServiceTypeReadRepository _serviceTypeReadRepository;

        public GetServiceTypeBySlugQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IServiceTypeReadRepository serviceTypeReadRepositor
        ) : base(authService, mapper, localizer)
        {
            _serviceTypeReadRepository = serviceTypeReadRepositor;
        }

        public async Task<ServiceTypeDto> Handle(GetServiceTypeBySlugQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Slug))
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var serviceType = await _serviceTypeReadRepository.GetTypeBySlugAndLangsAsync(request.Slug, request.Langs, cancellationToken);

            return _mapper.Map<ServiceTypeDto>(serviceType);
        }
    }
}
