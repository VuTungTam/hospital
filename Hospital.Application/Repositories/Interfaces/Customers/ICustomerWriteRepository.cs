﻿using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Customers;

namespace Hospital.Application.Repositories.Interfaces.Customers
{
    public interface ICustomerWriteRepository : IWriteRepository<Customer>
    {
        Task UpdateStatusAsync(Customer customer, CancellationToken cancellationToken);

        Task UpdateLastSeenAsync(CancellationToken cancellationToken = default);

        Task AddCustomerAsync(Customer customer, bool externalFlow = false, CancellationToken cancellationToken = default);
    }
}
