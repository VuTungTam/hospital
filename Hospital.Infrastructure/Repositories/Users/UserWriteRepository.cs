using Hospital.Application.Repositories.Interfaces.Auth.Roles;
using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Services.Auth.Entities;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Libraries.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Users
{
    public class UserWriteRepository : WriteRepository<User>, IUserWriteRepository
    {
        public UserWriteRepository(
            IServiceProvider serviceProvider, 
            IStringLocalizer<Resources> localizer, 
            IRedisCache redisCache
            ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task AddCustomerAsync(User customer, bool externalFlow = false, AccountStatus status = AccountStatus.UnConfirm, CancellationToken cancellationToken = default)
        {
            var roleReadRepository = _serviceProvider.GetRequiredService<IRoleReadRepository>();
            var sequenceRepository = _serviceProvider.GetRequiredService<ISequenceRepository>();
            var roles = await roleReadRepository.GetAsync(cancellationToken: cancellationToken);
            var table = "customer";
            var code = await sequenceRepository.GetSequenceAsync(table, cancellationToken);
            if (string.IsNullOrEmpty(customer.Password))
            {
                if (!externalFlow)
                {
                    var random = new Random();
                    customer.Password = "DefaultPassword123!";
                    customer.PasswordHash = customer.Password.ToMD5();
                }
                else
                {
                    customer.Password = "";
                    customer.PasswordHash = "";
                }
            }

            customer.Code = code.ValueString;
            customer.Salt = Utility.RandomString(6);
            customer.Username = customer.Email;
            customer.Status = status;
            customer.IsCustomer = true;
            customer.PasswordHash = customer.Password.ToMD5();
            customer.Password = "";
            if (customer.UserRoles == null || !customer.UserRoles.Any())
            {
                //customer.UserRoles = new List<UserRole> { new UserRole { RoleId = roles.First(x => x.Code == RoleCodeConstant.CUSTOMER).Id } };
            }

            Add(customer);
            await sequenceRepository.IncreaseValueAsync(table, cancellationToken);
        }

        public async Task AddEmployeeAsync(User user, CancellationToken cancellationToken)
        {
            user.Username = user.Email;
            user.PasswordHash = string.IsNullOrEmpty(user.Password) ? "" : user.Password.ToMD5();
            user.Salt = Utility.RandomString(6);
            user.IsCustomer = false;
            user.UserBranches = new List<UserBranch>
            {
                new UserBranch
                {
                    BranchId = _executionContext.BranchId
                }
            };
            await _dbSet.AddAsync(user, cancellationToken);
        }

        public async Task UpdateRolesAsync(long userId, IEnumerable<long> roleIds, CancellationToken cancellationToken)
        {
            var sql = $"DELETE FROM {new UserRole().GetTableName()} WHERE UserId = {userId}; ";
            foreach (var roleId in roleIds)
            {
                sql += $"INSERT INTO {new UserRole().GetTableName()}(RoleId, UserId, Creator, Created) VALUES ({roleId}, {userId}, {_executionContext.UserId}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'); ";
            }

            await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken);
        }

        public async Task UpdateStatusAsync(User user, CancellationToken cancellationToken)
        {
            _dbSet.Update(user);

            var entry = _dbContext.Entry(user);
            var properties = typeof(User).GetProperties().Where(p => p.GetIndexParameters().Length == 0 && p.PropertyType.IsPrimitive());
            foreach (var property in properties)
            {
                entry.Property(property.Name).IsModified = false;
            }
            entry.Property(x => x.Status).IsModified = true;
            entry.Property(x => x.EmailVerified).IsModified = true;
            entry.Property(x => x.PhoneVerified).IsModified = true;

            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
        }
    }
}
