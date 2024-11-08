using Hospital.Application.Dtos.Newes;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Newses
{
    public class GetNewsPagingQuery : BaseAllowAnonymousQuery<PagingResult<NewsDto>>
    {
        public GetNewsPagingQuery(Pagination pagination, NewsStatus status, DateTime postDate, bool clientSort, long excludeId)
        {
            Pagination = pagination;
            Status = status;
            PostDate = postDate;
            ClientSort = clientSort;
            ExcludeId = excludeId;
        }

        public Pagination Pagination { get; }
        public NewsStatus Status { get; }
        public DateTime PostDate { get; }
        public bool ClientSort { get; }
        public long ExcludeId { get; }
    }
}
