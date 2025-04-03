using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Modules.Notifications.Interfaces;
using Hospital.SharedKernel.Modules.Notifications.Models;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Notifications
{
    public class GetNotificationsCountQueryHandler : BaseQueryHandler, IRequestHandler<GetNotificationsCountQuery, NotificationCount>
    {
        private readonly INotificationReadRepository _notificationReadRepository;

        public GetNotificationsCountQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            INotificationReadRepository notificationReadRepository
        ) : base(authService, mapper, localizer)
        {
            _notificationReadRepository = notificationReadRepository;
        }

        public Task<NotificationCount> Handle(GetNotificationsCountQuery request, CancellationToken cancellationToken)
        {
            return _notificationReadRepository.GetCountAsync(cancellationToken);
        }
    }
}
