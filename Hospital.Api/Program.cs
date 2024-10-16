using FluentValidation;
using FluentValidation.AspNetCore;
using Hospital.Application.DI;
using Hospital.Infrastructure.DI;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Caching.Models;
using Hospital.SharedKernel.Configures;
using Hospital.SharedKernel.Domain.Constants;
using Hospital.SharedKernel.Runtime.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Hospital.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;


            // Add services to the container.
            CachingConfig.SetConfig(builder.Configuration);

            AuthConfig.Set(builder.Configuration);

            services.AddCoreService(builder.Configuration);

            services.AddCoreAuthentication(builder.Configuration);

            services.AddCoreCache(builder.Configuration);

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
