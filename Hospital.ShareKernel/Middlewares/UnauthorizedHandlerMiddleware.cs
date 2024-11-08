using Hospital.Resource.Properties;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Hospital.SharedKernel.Middlewares
{
    public class UnauthorizedHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<Resources>>();
                    await Response(localizer["auth_unauthorized"].Value);
                }
            }
            catch (Exception ex)
            {
                await Response(ex.Message);
            }

            async Task Response(string message)
            {
                if (context.Response.HasStarted)
                {
                    return;
                }

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                var body = new
                {
                    Message = message
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(body, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
            }
        }
    }

    public static class UnauthorizedHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseCoreUnauthorized(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UnauthorizedHandlerMiddleware>();
        }
    }
}
