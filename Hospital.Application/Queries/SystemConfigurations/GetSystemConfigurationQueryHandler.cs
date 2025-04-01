using AutoMapper;
using Hospital.Application.Dtos.SystemConfigurations;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Repositories.Interface.AppConfigs;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.SystemConfigurations
{
    public class GetSystemConfigurationQueryHandler : BaseQueryHandler, IRequestHandler<GetSystemConfigurationQuery, SystemConfigurationDto>
    {
        private readonly ISystemConfigurationReadRepository _systemConfigurationRepository;

        public GetSystemConfigurationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ISystemConfigurationReadRepository systemConfigurationRepository
        ) : base(authService, mapper, localizer)
        {
            _systemConfigurationRepository = systemConfigurationRepository;
        }

        public async Task<SystemConfigurationDto> Handle(GetSystemConfigurationQuery request, CancellationToken cancellationToken)
        {
            var sc = await _systemConfigurationRepository.GetAsync(cancellationToken: cancellationToken);
            return _mapper.Map<SystemConfigurationDto>(sc);
        }
    }
}
