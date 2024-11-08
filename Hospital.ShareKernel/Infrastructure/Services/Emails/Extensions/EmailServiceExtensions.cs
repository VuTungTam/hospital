using Hospital.SharedKernel.Infrastructure.Services.Emails.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.SharedKernel.Infrastructure.Services.Emails.Extensions
{
    public static class EmailServiceExtensions
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            EmailConfig.Set(configuration);
            return services.AddSingleton<IEmailService, EmailService>();
        }
    }
}
