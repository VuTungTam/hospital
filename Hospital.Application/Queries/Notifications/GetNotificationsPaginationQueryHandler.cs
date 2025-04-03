using AutoMapper;
using Hospital.Application.Dtos.Notifications;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Modules.Notifications.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Notifications
{
    public class GetNotificationsPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetNotificationsPaginationQuery, PaginationResult<NotificationDto>>
    {
        private readonly INotificationReadRepository _notificationReadRepository;

        public GetNotificationsPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            INotificationReadRepository notificationReadRepository
        ) : base(authService, mapper, localizer)
        {
            _notificationReadRepository = notificationReadRepository;
        }

        public async Task<PaginationResult<NotificationDto>> Handle(GetNotificationsPaginationQuery request, CancellationToken cancellationToken)
        {
            var result = await _notificationReadRepository.GetPaginationAsync(request.Pagination, spec: null, cancellationToken: cancellationToken);
            return new PaginationResult<NotificationDto>(_mapper.Map<List<NotificationDto>>(result.Data), result.Total);
        }
    }
}
