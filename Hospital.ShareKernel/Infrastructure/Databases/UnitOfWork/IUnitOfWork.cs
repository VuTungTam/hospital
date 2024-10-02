using Microsoft.EntityFrameworkCore.Storage;
namespace Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDbContextTransaction BeginTransaction();

        Task CommitAsync(bool dispatchEvent = false, CancellationToken cancellationToken = default);
    }
}
