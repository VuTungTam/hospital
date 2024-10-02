using FluentValidation;
using Hospital.Application.Dtos.SocialNetworks;
using Hospital.Application.Mappings;
using Hospital.Application.Services.Impls.Auth;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hospital.Application.DI
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            services.AddCommandHandlers();
            services.AddQueryHandlers();

            // Auth
            services.AddScoped<IAuthService, AuthService>();

            

            return services;
        }
    }
}
