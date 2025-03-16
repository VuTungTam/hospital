using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Ocelot.Infrastructure.Extensions;
using System.Net;

namespace Hospital.SharedKernel.Runtime.Filters
{
    public class AccessTokenValidatorAsyncFilter : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if (AuthUtility.EndpointRequiresAuthorize(context))
            {
                var bearerToken = context.HttpContext.Request.Headers[HeaderNames.Authorization];
                if (!string.IsNullOrEmpty(bearerToken))
                {
                    var accessToken = bearerToken.GetValue()[7..];
                    var executionContext = context.HttpContext.RequestServices.GetRequiredService<IExecutionContext>();
                    var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                    var tokens = await authService.GetLiveAccessTokensOfUserAsync(executionContext.Identity);
                            
                    // If the user is logged out
                    var token = tokens.Find(t => t.Value == accessToken);
                    if (token == null || token.Status == TokenStatus.LoggedOut)
                    {
                        context.Result = new ContentResult
                        {
                            StatusCode = (int)HttpStatusCode.Unauthorized,
                            ContentType = "application/json"
                        };
                        return;
                    }

                    switch (token.Status)
                    {
                        case TokenStatus.ForceLogout:
                        case TokenStatus.Banned:
                            throw new UnauthorizeException((object)"force_logout");

                        case TokenStatus.FetchNew:
                            throw new UnauthorizeException((object)"fetch_new");
                        default:
                            break;
                    }
                }
            }
            await next();
        }
    }
}
