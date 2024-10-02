using MediatR;

namespace Hospital.SharedKernel.Application.CQRS.Commands.Base
{
    public abstract class BaseCommand<TResponse> : IRequest<TResponse>
    {
    }
    public abstract class BaseCommand : IRequest
    {
    }
}
