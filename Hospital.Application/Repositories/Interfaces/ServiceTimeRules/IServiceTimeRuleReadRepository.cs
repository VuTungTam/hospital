using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.ServiceTimeRules
{
    public interface IServiceTimeRuleReadRepository : IReadRepository<ServiceTimeRule>
    {
        Task<int> GetMaxSlotAsync(long serviceId, DateTime date ,CancellationToken cancellationToken);
        Task<PaginationResult<ServiceTimeRule>> GetPagingWithFilterAsync(Pagination pagination, long? serviceId, 
            DayOfWeek? dayOfWeek, CancellationToken cancellationToken = default);
    }
}
