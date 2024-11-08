using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Runtime.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.SharedKernel.Middlewares
{
    public class TimezoneMiddleware
    {
        private readonly RequestDelegate _next;
        private const string TimeKey = "X-Client-Time";

        public TimezoneMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var dateService = context.RequestServices.GetRequiredService<IDateService>();
            try
            {
                var t = context.Request.Headers[TimeKey];
                if (!string.IsNullOrEmpty(t))
                {
                    var clientTime = Convert.ToInt64(t);
                    var now = DateTime.UtcNow;
                    var unix = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    var totalMilliSeconds = Convert.ToInt64((now - unix).TotalSeconds * 1000);
                    var difference = totalMilliSeconds - clientTime;

                    //var difference = Convert.ToInt64((now - nowUtc).TotalMilliseconds) - offset;

                    // Thời gian lệch nhau giữa 2 quốc gia bất kỳ tối đa là 12h
                    if (difference > 12 * 60 * 60 * 1000)
                    {
                        throw new BadRequestException("Invalid timezone!");
                    }
                    dateService.SetDifference(difference);
                }
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch
            {
                dateService.SetDifference(0);
            }
            finally
            {
                await _next(context);
            }
        }
    }

    public static class TimezoneMiddlewareExtension
    {
        public static IApplicationBuilder UseCoreTimezone(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TimezoneMiddleware>();
        }
    }
}
