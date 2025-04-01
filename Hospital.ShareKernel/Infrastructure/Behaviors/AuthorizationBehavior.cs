using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;

namespace Hospital.SharedKernel.Infrastructure.Behaviors
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IAuthService _authService;

        public AuthorizationBehavior(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is BaseQuery<TResponse> || request is BaseCommand<TResponse> || request is BaseCommand)
            {
                var attribute = (RequiredPermissionAttribute)request.GetType().GetCustomAttributes(typeof(RequiredPermissionAttribute), true).FirstOrDefault();
                if (attribute == null || !attribute.Exponents.Any())
                {
                    throw new CatchableException("No configured permission found");
                }

                var allowAnonymous = attribute.Exponents.Contains(ActionExponent.AllowAnonymous);
                if (!allowAnonymous)
                {
                    var exponents = attribute.Exponents;
                    if (attribute.HasAnyPermission)
                    {
                        if (!_authService.CheckHasAnyPermission(exponents))
                        {
                            throw new ForbiddenException();
                        }
                    }
                    else
                    {
                        if (!_authService.CheckPermission(exponents))
                        {
                            throw new ForbiddenException();
                        }
                    }
                }
            }
            return await next();
        }
    }
}
