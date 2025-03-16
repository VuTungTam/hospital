using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites;

namespace Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces
{
    public interface ILocationReadRepository
    {
        Task<PaginationResult<Province>> GetProvincesPagingAsync(Pagination pagination, CancellationToken cancellationToken);

        Task<PaginationResult<District>> GetDistrictsPagingAsync(int provinceId, Pagination pagination, CancellationToken cancellationToken);

        Task<PaginationResult<Ward>> GetWardsPagingAsync(int districtId, Pagination pagination, CancellationToken cancellationToken);

        Task<string> GetNameByIdAsync(int id, string type = "province", CancellationToken cancellationToken = default);

        Task<int> GetPidByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<int> GetDidByNameAsync(string name, int pid, CancellationToken cancellationToken = default);
        Task<int> GetWidByNameAsync(string name, int did, CancellationToken cancellationToken = default);
    }
}
