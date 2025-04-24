using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.Customers;
using Hospital.Domain.Specifications.Doctors;
using Hospital.Domain.Specifications.Employees;
using Hospital.Infrastructure.EFConfigurations;
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
            ISpecification<Employee> specEmployee = new EmployeeByPhoneEqualsSpecification(phone);
            ISpecification<Customer> specCustomer = new CustomerByPhoneEqualsSpecification(phone);
            ISpecification<Doctor> specDoctor = new DoctorByPhoneEqualsSpecification(phone);

            if (exceptId > 0)
            {
                specEmployee = specEmployee.Not(new IdEqualsSpecification<Employee>(exceptId));
                specCustomer = specCustomer.Not(new IdEqualsSpecification<Customer>(exceptId));
                specDoctor = specDoctor.Not(new IdEqualsSpecification<Doctor>(exceptId));
            }

            return await _dbContext.Employees.AnyAsync(specEmployee.GetExpression(), cancellationToken)
                || await _dbContext.Customers.AnyAsync(specCustomer.GetExpression(), cancellationToken)
                || await _dbContext.Doctors.AnyAsync(specDoctor.GetExpression(), cancellationToken);
        }


        public async Task<bool> EmailExistAsync(string email, long exceptId = 0, CancellationToken cancellationToken = default)
        {
            ISpecification<Employee> specEmployee = new EmployeeByEmailEqualsSpecification(email);
            ISpecification<Customer> specCustomer = new CustomerByEmailEqualsSpecification(email);
            ISpecification<Doctor> specDoctor = new DoctorByEmailEqualsSpecification(email);

            if (exceptId > 0)
            {
                specEmployee = specEmployee.Not(new IdEqualsSpecification<Employee>(exceptId));
                specCustomer = specCustomer.Not(new IdEqualsSpecification<Customer>(exceptId));
                specDoctor = specDoctor.Not(new IdEqualsSpecification<Doctor>(exceptId));
            }

            return await _dbContext.Employees.AnyAsync(specEmployee.GetExpression(), cancellationToken)
                || await _dbContext.Customers.AnyAsync(specCustomer.GetExpression(), cancellationToken)
                || await _dbContext.Doctors.AnyAsync(specDoctor.GetExpression(), cancellationToken);
        }


        public async Task<bool> CodeExistAsync(string code, long exceptId = 0, CancellationToken cancellationToken = default)
        {
            ISpecification<Employee> specEmployee = new EmployeeByCodeEqualsSpecification(code);
            ISpecification<Customer> specCustomer = new CustomerByCodeEqualsSpecification(code);
            ISpecification<Doctor> specDoctor = new DoctorByCodeEqualsSpecification(code);

            if (exceptId > 0)
            {
                specEmployee = specEmployee.Not(new IdEqualsSpecification<Employee>(exceptId));
                specCustomer = specCustomer.Not(new IdEqualsSpecification<Customer>(exceptId));
                specDoctor = specDoctor.Not(new IdEqualsSpecification<Doctor>(exceptId));
            }

            return await _dbContext.Employees.AnyAsync(specEmployee.GetExpression(), cancellationToken)
                || await _dbContext.Customers.AnyAsync(specCustomer.GetExpression(), cancellationToken)
                || await _dbContext.Doctors.AnyAsync(specDoctor.GetExpression(), cancellationToken);
        }

    }
}
