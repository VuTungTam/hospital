using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;
using MediatR;

namespace Hospital.SharedKernel.Application.CQRS.Commands.Base
{
    [RequiredPermission(new ActionExponent[] { ActionExponent.AllowAnonymous })]
    public abstract class BaseAllowAnonymousCommand<TResponse> : BaseCommand<TResponse>
    {
    }

    public abstract class BaseAllowAnonymousCommand : BaseAllowAnonymousCommand<Unit>
    {
    }
}
