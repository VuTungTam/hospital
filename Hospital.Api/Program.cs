using Hospital.Application.DI;
using Hospital.Infrastructure.DI;
using Hospital.SharedKernel.Configures;
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

            DefaultConfigSetup.Exec(builder.Configuration);

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
