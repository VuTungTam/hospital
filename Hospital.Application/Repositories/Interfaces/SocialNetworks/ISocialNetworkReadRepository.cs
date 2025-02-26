using Hospital.Domain.Entities.SocialNetworks;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.SocialNetworks
{
    public interface ISocialNetworkReadRepository : IReadRepository<SocialNetwork>
    {
        Task<PagingResult<SocialNetwork>> GetPaging(Pagination pagination, CancellationToken cancellationToken);
    }
}
