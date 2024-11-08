using Hospital.Resource.Properties;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hospital.SharedKernel.Middlewares
{
    public class TraceMiddleware
    {
        private readonly RequestDelegate _next;

        public TraceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var executionContext = context.RequestServices.GetRequiredService<IExecutionContext>();
                context.Response.Headers.Add("trace-id", executionContext.TraceId);
            }
            catch (UnauthorizeException exception)
            {
                var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<Resources>>();

                context.Response.StatusCode = 401;
                var body = new
                {
                    Message = localizer["auth_unauthorized"].Value,
                    Data = exception.DataException
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(body, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
                return;
            }
            catch (Exception)
            {
                throw;
            }

            await _next(context);
        }
    }

    public static class TraceMiddlewareExtension
    {
        public static IApplicationBuilder UseCoreTraceId(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TraceMiddleware>();
        }
    }
}
