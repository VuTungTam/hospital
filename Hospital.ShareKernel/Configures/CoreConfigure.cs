using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Hospital.SharedKernel.Application.Services.Date;
using MediatR;
using Hospital.SharedKernel.Infrastructure.Behaviors;
using Microsoft.AspNetCore.Hosting;
using Hospital.SharedKernel.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using StackExchange.Redis;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Hospital.SharedKernel.Caching.In_Memory;

namespace Hospital.SharedKernel.Configures
{
    public static class CoreConfigure
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton(_ => Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCoreLocalization();
            services.AddScoped<IDateService, DateService>();
            services.AddCoreBehaviors();
            services.AddCoreExecutionContext();
            return services;
        }
        public static IServiceCollection AddCoreLocalization(this IServiceCollection services)
        {
            var supportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider() };
                
            });

            return services;
        }

        public static IServiceCollection AddCoreExecutionContext(this IServiceCollection services)
        {
            return services.AddScoped<IExecutionContext, Runtime.ExecutionContext.ExecutionContext>();
        }

        public static IServiceCollection AddCoreBehaviors(this IServiceCollection services)
        {
            return services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
        #region Middlewares
        public static void UseCoreConfigure(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseCoreLocalization();

        }
        #endregion
        public static void UseCoreLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }
        public static IServiceCollection AddCoreCache(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configurationOptions = new ConfigurationOptions
                {
                    EndPoints = { $"{CachingConfig.Host}:{CachingConfig.Port}" },
                    Password = CachingConfig.Password,
                    DefaultDatabase = CachingConfig.DbNumber
                };
                return ConnectionMultiplexer.Connect(configurationOptions);
            });

            services.AddSingleton<IRedisCache, ConnectionMultiplexerRedis>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            return services;
        }
    }
}