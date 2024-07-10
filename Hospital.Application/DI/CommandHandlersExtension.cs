using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Hospital.Application.Dtos.Blogs;
using Hospital.Domain.Entities.Blogs;
using Hospital.SharedKernel.Application.CQRS.Queries;

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
