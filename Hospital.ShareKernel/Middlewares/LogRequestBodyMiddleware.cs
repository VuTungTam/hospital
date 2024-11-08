using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Text;

namespace Hospital.SharedKernel.Middlewares
{
    public class LogRequestBodyMiddleware
    {
        private readonly RequestDelegate _next;

        public LogRequestBodyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                var body = await reader.ReadToEndAsync();

                // Reset stream position để cho các middleware hoặc controller khác có thể đọc lại body từ đầu
                context.Request.Body.Position = 0;
                if (!string.IsNullOrEmpty(body))
                {
                    Log.Logger.Information("{Body}", body);
                }
            }

            await _next(context);
        }
    }

    public static class LogRequestBodyMiddlewareExtension
    {
        public static IApplicationBuilder UseCoreLogRequestBody(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LogRequestBodyMiddleware>();
        }
    }
}
