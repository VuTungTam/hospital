using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;
using MediatR;

namespace Hospital.SharedKernel.Application.CQRS.Queries.Base
{
    [RequiredPermission(ActionExponent.View)]
    public abstract class BaseQuery<TResponse> : IRequest<TResponse>
    {
    }
}
