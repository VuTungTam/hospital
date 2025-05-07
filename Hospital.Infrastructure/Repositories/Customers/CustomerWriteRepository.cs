using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Infrastructure.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Hospital.Infrastructure.Repositories.Customers
{
    public class CustomerWriteRepository : WriteRepository<Customer>, ICustomerWriteRepository
    {
        public static List<string> DefaultRandomPassword = new List<string>
        {
            "XukaYeuChaien",
            "NobitaDiHonda",
            "NobitaChaXeko",
            "DekhiYeuMimi",
            "DoremonDiLonTon",
            "ChaienBeNho",
            "XukaMoNhon"
        };

        public CustomerWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task AddCustomerAsync(Customer customer, bool externalFlow = false, CancellationToken cancellationToken = default)
        {
            var sequenceRepository = _serviceProvider.GetRequiredService<ISequenceRepository>();
            var table = new Customer().GetTableName();
            var code = await sequenceRepository.GetSequenceAsync(table, cancellationToken);

            if (string.IsNullOrEmpty(customer.Password))
            {
                customer.IsDefaultPassword = true;
                if (!externalFlow)
                {
                    var random = new Random();
                    customer.Password = DefaultRandomPassword[random.Next(0, DefaultRandomPassword.Count)];
                }
                else
                {
                    customer.Password = "";
                }
            }
            customer.HashPassword();

            if (customer.Dob == default || customer.Dob == DateTime.MinValue)
            {
                customer.Dob = null;
            }

            customer.Code = code.ValueString;
            customer.Status = customer.Status == AccountStatus.None ? AccountStatus.Active : customer.Status;
            customer.LastSeen = null;

            Add(customer);
            await sequenceRepository.IncreaseValueAsync(table, cancellationToken);
        }

        public async Task AddNotificationForCustomerAsync(Notification notification, long OwnerId, CallbackWrapper callbackWrapper, CancellationToken cancellationToken)
        {
            var query = _dbSet.AsNoTracking()
                .Where(x => x.Id == OwnerId);

            var customerReadRepository = _serviceProvider.GetRequiredService<ICustomerReadRepository>();
            var notificationWriteRepository = _serviceProvider.GetRequiredService<INotificationWriteRepository>();

            var customer = await query.FirstOrDefaultAsync(cancellationToken);

            var removeCacheTasks = new List<Task>();

            var noti = JsonConvert.DeserializeObject<Notification>(JsonConvert.SerializeObject(notification));

            noti.OwnerId = customer.Id;
            notificationWriteRepository.Add(noti);

            removeCacheTasks.Add(notificationWriteRepository.RemovePaginationCacheByUserIdAsync(customer.Id, cancellationToken));

            callbackWrapper.Callback = () => Task.WhenAll(removeCacheTasks);
        }

        public async Task UpdateStatusAsync(Customer customer, CancellationToken cancellationToken)
        {
            _dbSet.Update(customer);

            var entry = _dbContext.Entry(customer);
            var properties = typeof(Customer).GetProperties().Where(p => p.GetIndexParameters().Length == 0 && p.PropertyType.IsPrimitive());
            foreach (var property in properties)
            {
                entry.Property(property.Name).IsModified = false;
            }
            entry.Property(x => x.Status).IsModified = true;
            entry.Property(x => x.EmailVerified).IsModified = true;
            entry.Property(x => x.PhoneVerified).IsModified = true;

            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
        }

        public async Task UpdateLastSeenAsync(CancellationToken cancellationToken = default)
        {
            var sql = $"UPDATE {new Customer().GetTableName()} SET LastSeen = GETDATE() WHERE Id = {_executionContext.Identity}; ";

            await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
            await _dbContext.CommitAsync(cancellationToken: cancellationToken);
        }
    }
}
