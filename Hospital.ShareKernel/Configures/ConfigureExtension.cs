using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Hospital.SharedKernel.Application.Services.Date;

namespace Hospital.SharedKernel.Configures
{
    public static class ConfigureExtension
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton(_ => Configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCoreLocalization();
            services.AddScoped<IDateService, DateService>();
            return services;
        }
        public static IServiceCollection AddCoreLocalization(this IServiceCollection services)
        {
            var supportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(culture: "vi-VN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider() };
            });

            return services;
        }
    }
}
