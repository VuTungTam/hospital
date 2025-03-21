using Hospital.SharedKernel.Domain.Events.BaseEvents;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Serilog;
using System.Diagnostics;

namespace Hospital.Infrastructure.Events.Dispatchers
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IMediator _mediator;

        public EventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task FireEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IDomainEvent
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await _mediator.Publish(@event, cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Failed to publish event {Exception}", ex);
                throw;
            }
            finally
            {
                sw.Stop();
                Log.Logger.Debug($"Fire event [{@event.EventType}] [{@event.EventId}] at {@event.Timestamp} took {sw.ElapsedMilliseconds}ms ");
            }
        }
    }
}
//WebRTC