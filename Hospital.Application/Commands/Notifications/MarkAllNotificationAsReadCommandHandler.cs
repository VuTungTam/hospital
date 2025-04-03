using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Modules.Notifications.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Notifications
{
    public class MarkAllNotificationAsReadCommandHandler : BaseCommandHandler, IRequestHandler<MarkAllNotificationAsReadCommand>
    {
        private readonly INotificationWriteRepository _notificationWriteRepository;

        public MarkAllNotificationAsReadCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            INotificationWriteRepository notificationWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _notificationWriteRepository = notificationWriteRepository;
        }

        public async Task<Unit> Handle(MarkAllNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            await _notificationWriteRepository.MarkAllAsReadAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
