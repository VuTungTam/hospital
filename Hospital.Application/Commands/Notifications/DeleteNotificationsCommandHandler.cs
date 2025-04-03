using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Modules.Notifications.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Notifications
{
    public class DeleteNotificationsCommandHandler : BaseCommandHandler, IRequestHandler<DeleteNotificationsCommand>
    {
        private readonly INotificationWriteRepository _notificationWriteRepository;

        public DeleteNotificationsCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            INotificationWriteRepository notificationWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _notificationWriteRepository = notificationWriteRepository;
        }

        public async Task<Unit> Handle(DeleteNotificationsCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || !request.Ids.Any())
            {
                throw new BadRequestException(_localizer["CommonMessage.ListIdsMustNotBeEmpty"]);
            }

            await _notificationWriteRepository.DeleteAsync(request.Ids, cancellationToken);

            return Unit.Value;
        }
    }
}
