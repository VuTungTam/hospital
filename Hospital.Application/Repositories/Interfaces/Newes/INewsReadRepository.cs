using Hospital.Domain.Entities.Newses;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Newes
{
    public interface INewsReadRepository : IReadRepository<News>
    {
        Task<News> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task<PagingResult<News>> GetPagingWithFilterAsync(Pagination pagination, NewsStatus status, long excludeId = 0, DateTime postDate = default, bool clientSort = false, CancellationToken cancellationToken = default);
    }
}
