using Hospital.Application.Mappings;
using Hospital.Application.Services.Impls.Accounts;
using Hospital.Application.Services.Impls.Auth;
using Hospital.Application.Services.Impls.Sockets;
using Hospital.Application.Services.Interfaces.Sockets;
using Hospital.SharedKernel.Application.Services.Accounts.Interfaces;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Extensions;
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

            // Email
            EmailServiceExtensions.AddEmailService(services, configuration);

            // Auth
            services.AddScoped<IAuthService, AuthService>();

            // Accounts
            services.AddScoped<IAccountService, AccountService>();

            //Socket
            services.AddScoped<ISocketService, SocketService>();


            return services;
        }
    }
}
