using MediatR;

namespace Hospital.SharedKernel.Application.CQRS.Queries.Base
{
    public abstract class BaseQuery<TResponse> : IRequest<TResponse>
    {
    }
}
