using Hospital.Application.Services.Interfaces.Sockets;
using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Hospital.Application.Services.Impls.Sockets
{
    public class SocketService : ISocketService
    {
        public IHubContext<SignalRHub> HubContext => throw new NotImplementedException();

        public Task AskReload(long userId, string message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task ForceLogout(long userId, string message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Maintaince(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SendNewBooking(Booking booking, string branchName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
