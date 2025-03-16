using Hospital.Application.Dtos.Articles;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.Articles
{
    public class GetArticlesPaginationQuery : BaseAllowAnonymousQuery<PaginationResult<ArticleDto>>
    {
        public GetArticlesPaginationQuery(Pagination pagination, ArticleStatus status, DateTime postDate, bool clientSort, long excludeId)
        {
            Pagination = pagination;
            Status = status;
            PostDate = postDate;
            ClientSort = clientSort;
            ExcludeId = excludeId;
        }

        public Pagination Pagination { get; }
        public ArticleStatus Status { get; }
        public DateTime PostDate { get; }
        public bool ClientSort { get; }
        public long ExcludeId { get; }
    }
}
