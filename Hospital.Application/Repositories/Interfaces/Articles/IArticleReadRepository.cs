using Hospital.Domain.Entities.Articles;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Articles
{
    public interface IArticleReadRepository : IReadRepository<Article>
    {
        Task<bool> IsSlugExistsAsync(long excludeId, string slug, CancellationToken cancellationToken);

        Task<Article> GetBySlugAndLangsAsync(string slug, List<string> langs, CancellationToken cancellationToken = default);

        Task<int> GetViewCountAsync(long id, CancellationToken cancellationToken = default);

        Task<PaginationResult<Article>> GetPaginationWithFilterAsync(Pagination pagination, ArticleStatus status, long excludeId = 0, DateTime postDate = default, bool clientSort = false, CancellationToken cancellationToken = default);
    }
}
