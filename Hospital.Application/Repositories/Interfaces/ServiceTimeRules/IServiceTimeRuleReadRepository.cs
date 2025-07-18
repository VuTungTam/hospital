﻿using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.ServiceTimeRules
{
    public interface IServiceTimeRuleReadRepository : IReadRepository<ServiceTimeRule>
    {
        Task<PaginationResult<ServiceTimeRule>> GetPagingWithFilterAsync(Pagination pagination, long serviceId,
            int dayOfWeek, CancellationToken cancellationToken = default);
        Task<List<ServiceTimeRule>> GetByServiceIdAsync(long serviceId, CancellationToken cancellationToken);
    }
}
