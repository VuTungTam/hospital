using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Repositories.Interface.AppConfigs;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Systems;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.SystemConfigurations
{
    public class UpdateSystemConfigurationCommandHandler : BaseCommandHandler, IRequestHandler<UpdateSystemConfigurationCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly ISystemConfigurationReadRepository _systemConfigurationReadRepository;
        private readonly ISystemConfigurationWriteRepository _systemConfigurationWriteRepository;

        public UpdateSystemConfigurationCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            ISystemConfigurationReadRepository systemConfigurationReadRepository,
            ISystemConfigurationWriteRepository systemConfigurationWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _executionContext = executionContext;
            _systemConfigurationReadRepository = systemConfigurationReadRepository;
            _systemConfigurationWriteRepository = systemConfigurationWriteRepository;
        }

        public async Task<Unit> Handle(UpdateSystemConfigurationCommand request, CancellationToken cancellationToken)
        {
            var entity = await _systemConfigurationReadRepository.GetAsync(cancellationToken);
            if (entity == null)
            {
                throw new CatchableException("System config not found");
            }

            var config = _mapper.Map<SystemConfiguration>(request.SystemConfigurationDto);
            var bccList = request.SystemConfigurationDto.BookingNotificationBccEmails;

            if (bccList != null && bccList.Any())
            {
                var bccEmails = bccList.Select(x => x.Trim()).Distinct().ToList();
                config.BookingNotificationBccEmails = string.Join(",", bccEmails);
            }

            //config.AddDomainEvent(new UpdateSystemConfigurationDomainEvent(entity, request.SystemConfigurationDto));

            await _systemConfigurationWriteRepository.SaveAsync(config, cancellationToken);

            return Unit.Value;
        }
    }
}
