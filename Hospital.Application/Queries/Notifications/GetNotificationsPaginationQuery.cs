using Hospital.Application.Dtos.Notifications;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Notifications
{
    public class GetNotificationsPaginationQuery : BaseQuery<PaginationResult<NotificationDto>>
    {
        public GetNotificationsPaginationQuery(Pagination pagination)
        {
            Pagination = pagination;
        }

        public Pagination Pagination { get; }
    }
}
