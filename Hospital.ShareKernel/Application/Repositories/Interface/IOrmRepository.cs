using Hospital.SharedKernel.Application.Repositories.Models;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Specifications.Interfaces;

namespace Hospital.SharedKernel.Application.Repositories.Interface
{
    public interface IOrmRepository
    {
        ISpecification<T> GuardDataAccess<T>(ISpecification<T> spec, bool ignoreOwner = false, bool ignoreBranch = false) where T : BaseEntity;

        Task<T> FindBySpecificationAsync<T>(ISpecification<T> spec, string[] includes = default, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default) where T : BaseEntity;

        Task<List<T>> GetBySpecificationAsync<T>(ISpecification<T> spec, string[] includes = default, bool ignoreOwner = false, bool ignoreBranch = false, CancellationToken cancellationToken = default) where T : BaseEntity;
    }
}
