using FluentValidation;
using Hospital.Application.Dtos.SocialNetworks;
using Hospital.Application.Mappings;
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
            
            return services;
        }
    }
}
