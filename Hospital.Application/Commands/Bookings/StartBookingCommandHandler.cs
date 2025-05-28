using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Services.Interfaces.Sockets;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Enums;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class StartBookingCommandHandler : BaseCommandHandler, IRequestHandler<StartBookingCommand>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IRedisCache _redisCache;
        private readonly IDateService _dateService;
        private readonly ISocketService _socketService;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        public StartBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            ISocketService socketService,
            IMapper mapper,
            IDateService dateService,
            ICustomerWriteRepository customerWriteRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _redisCache = redisCache;
            _dateService = dateService;
            _socketService = socketService;
            _customerWriteRepository = customerWriteRepository;
        }

        public async Task<Unit> Handle(StartBookingCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var option = new QueryOption
            {
                IgnoreOwner = true
            };

            var booking = await _bookingReadRepository.GetByIdAsync(request.Id, option, cancellationToken: cancellationToken);
            if (booking == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            if (booking.Status != BookingStatus.Confirmed)
            {
                throw new BadRequestException(_localizer["booking_status_is_not_confirmed"]);
            }

            booking.StartBooking = _dateService.GetClientTime().TimeOfDay;

            booking.Status = BookingStatus.Doing;

            var nextBookings = await _bookingReadRepository.GetNextBookings(booking, cancellationToken);

            var callbackWrapper = new CallbackWrapper();

            Booking notiBooking = null;

            if (nextBookings != null && nextBookings.Any())
            {
                notiBooking = nextBookings.First();
                foreach (var nextBooking in nextBookings)
                {
                    var notification = new Notification
                    {
                        Id = AuthUtility.GenerateSnowflakeId(),
                        Data = nextBooking.Id.ToString(),
                        IsUnread = true,
                        Description = $"<p>Số thứ tự hiện tại là <span class='n-bold'>{booking.Order}</span>, còn {nextBooking.Order - booking.Order} người nữa trước bạn.</p>",
                        Timestamp = DateTime.Now,
                        Type = NotificationType.Remind
                    };
                    await _customerWriteRepository.AddNotificationForCustomerAsync(notification, nextBooking.OwnerId, callbackWrapper, cancellationToken);
                }
            }

            await _bookingWriteRepository.UpdateAsync(booking, cancellationToken: cancellationToken);
            if (notiBooking != null)
            {
                await _socketService.NextBooking(notiBooking, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
