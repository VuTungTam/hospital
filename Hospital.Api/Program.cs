using Hospital.Application.DI;
using Hospital.Domain.Configs;
using Hospital.Infrastructure.DI;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Caching.Models;
using Hospital.SharedKernel.Configures;
using Hospital.SharedKernel.Configures.Models;
using Hospital.SharedKernel.Runtime.Filters;
using Microsoft.AspNetCore.HttpOverrides;

namespace Hospital.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;


            // Add services to the container.
            CdnConfig.SetConfig(builder.Configuration); 

            CachingConfig.SetConfig(builder.Configuration);

            FeatureConfig.SetConfig(builder.Configuration);

            AuthConfig.Set(builder.Configuration);

            services.AddCoreService(builder.Configuration);

            services.AddCoreAuthentication(builder.Configuration);

            services.AddCoreCache(builder.Configuration);

            services.AddCloudinary(builder.Configuration);

            services.Configure<ForwardedHeadersOptions>(o => o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

            services.AddCoreExecutionContext();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AccessTokenValidatorAsyncFilter());
            });

            services.AddApplicationServices(builder.Configuration);

            services.AddInfrastructureService(builder.Configuration);

            services.AddHttpClient();

            var app = builder.Build();

            app.UseCoreCors(builder.Configuration);

            app.UseCoreConfigure(app.Environment);

            await app.RunAsync();
        }
    }
}
