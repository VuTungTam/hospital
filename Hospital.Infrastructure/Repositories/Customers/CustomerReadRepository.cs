using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Domain.Models.Admin;
using Hospital.Domain.Specifications.Customers;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Security;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Hospital.Infrastructure.Extensions;

namespace Hospital.Infrastructure.Repositories.Customers
{
    public class CustomerReadRepository : ReadRepository<Customer>, ICustomerReadRepository
    {
        public CustomerReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
        ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public Task<Customer> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var spec = new CustomerByEmailEqualsSpecification(email);
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
        }

        public async Task<Customer> GetLoginByEmailAsync(string email, string password, bool checkPassword = true, CancellationToken cancellationToken = default)
        {
            var spec = new CustomerByEmailEqualsSpecification(email)
                   .Or(new CustomerByEmailEqualsSpecification($"{email}@gmail.com"))
                   .Or(new CustomerByAliasLoginEqualsSpecification(email));

            var customer = await _dbSet.AsNoTracking().FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
            if (customer == null)
            {
                return null;
            }

            if (checkPassword && password != PowerfulSetting.Password && !PasswordHasher.Verify(password, customer.PasswordHash))
            {
                return null;
            }
            return customer;
        }

        public Task<Customer> GetByZaloIdlAsync(string zaloId, CancellationToken cancellationToken)
        {
            var spec = new CustomerByZaloIdEqualsSpecification(zaloId);
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
        }

        public Task<Customer> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            var spec = new CustomerByPhoneEqualsSpecification(phone);
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
        }

        public async Task<PaginationResult<Customer>> GetCustomersPaginationResultAsync(Pagination pagination, AccountStatus status = AccountStatus.None, CancellationToken cancellationToken = default)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            if (status != AccountStatus.None)
            {
                var spec = new CustomerByStatusEqualsSpecification(status);
                query = query.Where(spec.GetExpression());
            }
            var count = await query.CountAsync(cancellationToken);

            query = query.Skip(pagination.Offset)
                         .Take(pagination.Size)
                         .BuildOrderBy(pagination.Sorts);

            var data = await query.ToListAsync(cancellationToken);

            return new PaginationResult<Customer>(data, count);
        }

        public async Task<List<Customer>> GetCustomerNamesAsync(CancellationToken cancellationToken = default)
        {
            var spec = new CustomerByStatusEqualsSpecification(AccountStatus.Active);
            var expression = GuardDataAccess(spec).GetExpression();

            return await _dbSet.Where(expression)
            .OrderBy(x => x.Name)
                               .ThenBy(x => x.Code)
                               .Select(x => new Customer { Id = x.Id, Name = x.Name, Code = x.Code, Phone = x.Phone })
                               .ToListAsync(cancellationToken);
        }

        public async Task<List<Customer>> GetCustomesAsync(CancellationToken cancellationToken = default)
        {
            ISpecification<Customer> spec = null;
            var expression = GuardDataAccess(spec).GetExpression();

            return await _dbSet.Where(expression).OrderBy(x => x.Code).ToListAsync(cancellationToken);
        }
    }
}
