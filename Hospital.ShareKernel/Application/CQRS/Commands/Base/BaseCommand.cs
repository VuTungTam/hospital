using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;
using MediatR;

namespace Hospital.SharedKernel.Application.CQRS.Commands.Base
{
    [RequiredPermission(ActionExponent.View)]

    public abstract class BaseCommand<TResponse> : IRequest<TResponse>
    {
    }
    [RequiredPermission(ActionExponent.View)]
    public abstract class BaseCommand : IRequest
    {
    }
}
