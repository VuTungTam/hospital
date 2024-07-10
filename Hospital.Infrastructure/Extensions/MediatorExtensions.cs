using MediatR;
using Hospital.Infra.EFConfigurations;
using Hospital.SharedKernel.Domain.Entities.Base;

namespace Hospital.Infra.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, AppDbContext context, CancellationToken cancellationToken)
        {
            var entries = context.ChangeTracker
                                 .Entries<BaseEntity>()
                                 .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());
            var events = entries.Select(x => x.Entity.DomainEvents).ToList();

            foreach (var entity in entries)
            {
                entity.Entity.ClearDomainEvents();
            }

            var tasks = events.Select(@event => mediator.Publish(@event));
            await Task.WhenAll(tasks);
        }
    }
}
