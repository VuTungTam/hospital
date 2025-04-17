using Hospital.Domain.Entities.Metas;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Metas
{
    public interface IScriptReadRepository : IReadRepository<Script>
    {
        Task<Script> ReadAsync(CancellationToken cancellationToken);
    }
}
