using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites;

namespace Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces
{
    public interface ILocationReadRepository
    {
        Task<PagingResult<Province>> GetProvincesPagingAsync(Pagination pagination, CancellationToken cancellationToken);

        Task<PagingResult<District>> GetDistrictsPagingAsync(int provinceId, Pagination pagination, CancellationToken cancellationToken);

        Task<PagingResult<Ward>> GetWardsPagingAsync(int districtId, Pagination pagination, CancellationToken cancellationToken);

        Task<string> GetNameByIdAsync(int id, string type = "province", CancellationToken cancellationToken = default);
    }
}
