using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.Customers;
using Hospital.Domain.Specifications.Employees;
using Hospital.Infra.EFConfigurations;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> PhoneExistAsync(string phone, long exceptId = 0, CancellationToken cancellationToken = default)
        {
            ISpecification<Employee> spec = new EmployeeByPhoneEqualsSpecification(phone);
            ISpecification<Customer> spec2 = new CustomerByPhoneEqualsSpecification(phone);

            if (exceptId > 0)
            {
                spec = spec.Not(new IdEqualsSpecification<Employee>(exceptId));
                spec2 = spec2.Not(new IdEqualsSpecification<Customer>(exceptId));
            }

            return await _dbContext.Employees.AnyAsync(spec.GetExpression(), cancellationToken) ||
                   await _dbContext.Customers.AnyAsync(spec2.GetExpression(), cancellationToken);
        }

        public async Task<bool> EmailExistAsync(string email, long exceptId = 0, CancellationToken cancellationToken = default)
        {
            ISpecification<Employee> spec = new EmployeeByEmailEqualsSpecification(email);
            ISpecification<Customer> spec2 = new CustomerByEmailEqualsSpecification(email);

            if (exceptId > 0)
            {
                spec = spec.Not(new IdEqualsSpecification<Employee>(exceptId));
                spec2 = spec2.Not(new IdEqualsSpecification<Customer>(exceptId));
            }

            return await _dbContext.Employees.AnyAsync(spec.GetExpression(), cancellationToken) ||
                   await _dbContext.Customers.AnyAsync(spec2.GetExpression(), cancellationToken);
        }

        public async Task<bool> CodeExistAsync(string code, long exceptId = 0, CancellationToken cancellationToken = default)
        {
            ISpecification<Employee> spec = new EmployeeByCodeEqualsSpecification(code);
            ISpecification<Customer> spec2 = new CustomerByCodeEqualsSpecification(code);

            if (exceptId > 0)
            {
                spec = spec.Not(new IdEqualsSpecification<Employee>(exceptId));
                spec2 = spec2.Not(new IdEqualsSpecification<Customer>(exceptId));
            }

            return await _dbContext.Employees.AnyAsync(spec.GetExpression(), cancellationToken) ||
                   await _dbContext.Customers.AnyAsync(spec2.GetExpression(), cancellationToken);
        }
    }
}
