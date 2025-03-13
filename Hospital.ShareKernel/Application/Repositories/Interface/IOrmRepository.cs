using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Specifications.Interfaces;

namespace Hospital.SharedKernel.Application.Repositories.Interface
{
    public interface IOrmRepository
    {
        ISpecification<T> GuardDataAccess<T>(ISpecification<T> spec, QueryOption option) where T : BaseEntity;

        Task<T> FindBySpecificationAsync<T>(ISpecification<T> spec, QueryOption option, CancellationToken cancellationToken = default) where T : BaseEntity;

        Task<List<T>> GetBySpecificationAsync<T>(ISpecification<T> spec, QueryOption option, CancellationToken cancellationToken = default) where T : BaseEntity;
    }
}
