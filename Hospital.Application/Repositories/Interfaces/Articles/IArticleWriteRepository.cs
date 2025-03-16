using Hospital.Domain.Entities.Articles;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Articles
{
    public interface IArticleWriteRepository : IWriteRepository<Article>
    {
        Task IncreaseViewCountAsync(long id, CancellationToken cancellationToken);
    }
}
