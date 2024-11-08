using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Application.DI
{
    public static class CommandHandlersExtension
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            return services;
        }
    }
}
