using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Domain.Entities.Base;

namespace Hospital.SharedKernel.Application.CQRS.Queries
{
    public class GetCountQuery<T> : BaseQuery<int> where T : BaseEntity
    {
    }
}
