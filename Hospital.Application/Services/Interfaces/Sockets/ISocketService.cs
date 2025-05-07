using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Presentations.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Hospital.Application.Services.Interfaces.Sockets
{
    public interface ISocketService
    {
        IHubContext<SignalRHub> HubContext { get; }

        Task Maintaince(CancellationToken cancellationToken = default);

        Task ForceLogout(long userId, string message, CancellationToken cancellationToken = default);

        Task AskReload(long userId, string message, CancellationToken cancellationToken = default);

        Task SendNewBooking(Booking booking, CancellationToken cancellationToken = default);

        Task ConfirmBooking(Booking booking, CancellationToken cancellationToken = default);

        Task CustomerCancelBooking(Booking booking, CancellationToken cancellationToken = default);

        Task EmployeeCancelBooking(Booking booking, CancellationToken cancellationToken = default);
    }
}
