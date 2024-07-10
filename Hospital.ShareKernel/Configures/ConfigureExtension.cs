using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Hospital.SharedKernel.Configures
{
    public static class ConfigureExtension
    {
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
