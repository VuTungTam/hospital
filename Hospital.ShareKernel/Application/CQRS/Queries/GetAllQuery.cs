using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Domain.Entities.Base;

namespace Hospital.SharedKernel.Application.CQRS.Queries
{
    public class GetAllQuery<T,TResponse> : BaseQuery<List<TResponse>> where T : BaseEntity
    {

    }
}
