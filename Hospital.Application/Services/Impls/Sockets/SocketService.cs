using Hospital.Application.Services.Interfaces.Sockets;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Presentations.SignalR;
using Hospital.SharedKernel.Presentations.SignalR.Models;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hospital.Application.Services.Impls.Sockets
{
    public class SocketService : ISocketService
    {
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IExecutionContext _executionContext;

        public SocketService(IHubContext<SignalRHub> hubContext, IExecutionContext executionContext, IAuthService authService)
        {
            _hubContext = hubContext;
            _executionContext = executionContext;
        }

        public IHubContext<SignalRHub> HubContext => _hubContext;

        public async Task Maintaince(CancellationToken cancellationToken = default)
        {
            var msg = new SignalRMessage { Type = (int)MessageHubType.Maintaince };
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", msg, cancellationToken);
        }

        public async Task ForceLogout(long userId, string message, CancellationToken cancellationToken = default)
        {
            var key = CacheManager.GetConnectionSocketKey(userId);
            if (
                !SignalRHub.EmployeeConnections.TryGetValue(key, out var connection) &&
                !SignalRHub.CustomerConnections.TryGetValue(key, out connection)
            )
            {
                var msg2 = new SignalRMessage { Type = (int)MessageHubType.FindLogout, Data = userId, Message = message };
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", msg2, cancellationToken);
                return;
            }

            var msg = new SignalRMessage { Type = (int)MessageHubType.Logout, Message = message };
            var connectionIds = connection.Select(c => c.ConnectionId);

            await _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveMessage", msg, cancellationToken);

            SignalRHub.EmployeeConnections.TryRemove(key, out var v);
            SignalRHub.CustomerConnections.TryRemove(key, out var v2);
        }

        public async Task AskReload(long userId, string message, CancellationToken cancellationToken = default)
        {
            var key = CacheManager.GetConnectionSocketKey(userId);
            if (
                !SignalRHub.EmployeeConnections.TryGetValue(key, out var connection) &&
                !SignalRHub.CustomerConnections.TryGetValue(key, out connection)
            )
            {
                return;
            }

            var msg = new SignalRMessage { Type = (int)MessageHubType.Reload, Message = message };
            var connectionIds = connection.Select(c => c.ConnectionId);

            await _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveMessage", msg, cancellationToken);
        }

        public async Task SendNewBooking(Booking booking, CancellationToken cancellationToken = default)
        {
            var employees = SignalRHub.EmployeeConnections.Values.Select(x => x);
            var connections = new List<AuthenticatedSocketConnection>();
            var connectionIds = new List<string>();

            foreach (var c in employees)
            {
                connectionIds.AddRange(c.Where(x => x.FacilityId == booking.FacilityId && x.ZoneId == booking.ZoneId).Select(x => x.ConnectionId));
            }

            var data = JsonConvert.SerializeObject(new
            {
                BookingId = booking.Id.ToString(),
            }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            var msg = new SignalRMessage { Type = (int)MessageHubType.Booking, Data = data };

            await _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveMessage", msg, cancellationToken);
        }

        public async Task ConfirmBooking(Booking booking, CancellationToken cancellationToken = default)
        {
            var customer = SignalRHub.CustomerConnections.Values.Select(x => x);
            var connections = new List<AuthenticatedSocketConnection>();
            var connectionIds = new List<string>();

            foreach (var c in customer)
            {
                connectionIds.AddRange(c.Where(x => x.UserId == booking.OwnerId.ToString()).Select(x => x.ConnectionId));
            }

            var data = JsonConvert.SerializeObject(new
            {
                BookingId = booking.Id.ToString(),
            }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            var msg = new SignalRMessage { Type = (int)MessageHubType.ConfirmBooking, Data = data };

            await _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveMessage", msg, cancellationToken);
        }

        public async Task CustomerCancelBooking(Booking booking, CancellationToken cancellationToken = default)
        {
            var connections = new List<AuthenticatedSocketConnection>();
            var connectionIds = new List<string>();
            var employees = SignalRHub.EmployeeConnections.Values.Select(x => x);
            foreach (var c in employees)
            {
                connectionIds.AddRange(c.Where(x => x.FacilityId == booking.FacilityId && x.ZoneId == booking.ZoneId).Select(x => x.ConnectionId));
            }

            var data = JsonConvert.SerializeObject(new
            {
                BookingId = booking.Id.ToString(),
            }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            var msg = new SignalRMessage { Type = (int)MessageHubType.CustomerCancelBooking, Data = data };

            await _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveMessage", msg, cancellationToken);
        }

        public async Task EmployeeCancelBooking(Booking booking, CancellationToken cancellationToken = default)
        {
            var connections = new List<AuthenticatedSocketConnection>();
            var connectionIds = new List<string>();
            var customer = SignalRHub.CustomerConnections.Values.Select(x => x);

            foreach (var c in customer)
            {
                connectionIds.AddRange(c.Where(x => x.UserId == booking.OwnerId.ToString()).Select(x => x.ConnectionId));
            }

            var data = JsonConvert.SerializeObject(new
            {
                BookingId = booking.Id.ToString(),
            }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            var msg = new SignalRMessage { Type = (int)MessageHubType.EmployeeCancelBooking, Data = data };

            await _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveMessage", msg, cancellationToken);
        }

        public async Task CompleteBooking(Booking booking, CancellationToken cancellationToken = default)
        {
            var customer = SignalRHub.CustomerConnections.Values.Select(x => x);
            var connections = new List<AuthenticatedSocketConnection>();
            var connectionIds = new List<string>();

            foreach (var c in customer)
            {
                connectionIds.AddRange(c.Where(x => x.UserId == booking.OwnerId.ToString()).Select(x => x.ConnectionId));
            }

            var data = JsonConvert.SerializeObject(new
            {
                BookingId = booking.Id.ToString(),
            }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            var msg = new SignalRMessage { Type = (int)MessageHubType.CompleteBooking, Data = data };

            await _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveMessage", msg, cancellationToken);
        }

        public async Task NextBooking(Booking booking, CancellationToken cancellationToken = default)
        {
            var customer = SignalRHub.CustomerConnections.Values.Select(x => x);
            var connections = new List<AuthenticatedSocketConnection>();
            var connectionIds = new List<string>();

            foreach (var c in customer)
            {
                connectionIds.AddRange(c.Where(x => x.UserId == booking.OwnerId.ToString()).Select(x => x.ConnectionId));
            }

            var data = JsonConvert.SerializeObject(new
            {
                BookingId = booking.Id.ToString(),
            }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            var msg = new SignalRMessage { Type = (int)MessageHubType.NextBooking, Data = data };

            await _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveMessage", msg, cancellationToken);
        }
    }
}
