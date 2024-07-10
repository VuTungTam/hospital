using Microsoft.Extensions.DependencyInjection;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Application.Repositories.Models;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Specifications.Interfaces;
using System.Threading;
using System;
using Hospital.Infra.EFConfigurations;

namespace Hospital.Infra.Repositories
{
    public class OrmRepository : IOrmRepository
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly AppDbContext _dbContext;
        public OrmRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            
        }

        public Task<T> FindBySpecificationAsync<T>(ISpecification<T> spec, QueryOption option = null, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetBySpecificationAsync<T>(ISpecification<T> spec, QueryOption option = null, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            throw new NotImplementedException();
        }

        public ISpecification<T> GuardDataAccess<T>(ISpecification<T> spec) where T : BaseEntity
        {
            throw new NotImplementedException();
        }
    }
}
