using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.SharedKernel.Application.CQRS.Queries.Base
{
    [RequiredPermission(new ActionExponent[] { ActionExponent.AllowAnonymous })]
    public class BaseAllowAnonymousQuery<TResponse> : BaseQuery<TResponse>
    {
    }
}
