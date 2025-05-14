using Hospital.Application.Models.Dashboards.Articles;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Dashboards.Articles
{
    [RequiredPermission(ActionExponent.ViewDashboard)]
    public class GetArticleStatsQuery : BaseQuery<ArticleStats>
    {
        public GetArticleStatsQuery(int top)
        {
            Top = top;
        }

        public int Top { get; }
    }
}
