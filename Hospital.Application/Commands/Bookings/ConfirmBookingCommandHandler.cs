using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Services.Interfaces.Sockets;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.ServiceTimeRules;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Enums;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class ConfirmBookingCommandHandler : BaseCommandHandler, IRequestHandler<ConfirmBookingCommand>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        private readonly IRedisCache _redisCache;
        private readonly ISocketService _socketService;
        public ConfirmBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository,
            ICustomerWriteRepository customerWriteRepository,
            ISocketService socketService,
            IRedisCache redisCache
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _redisCache = redisCache;
            _socketService = socketService;
            _customerWriteRepository = customerWriteRepository;
        }

        public async Task<Unit> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var option = new QueryOption
            {
                IgnoreOwner = true,
            };

            var booking = await _bookingReadRepository.GetByIdAsync(request.Id, option, cancellationToken: cancellationToken);

            if (booking == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            if (booking.Status != BookingStatus.Waiting)
            {
                throw new BadRequestException(_localizer["booking_status_is_not_waiting"]);
            }

            var maxOrder = await _bookingReadRepository.GetMaxOrderAsync(booking.ServiceId, booking.Date,
                booking.TimeSlotId, cancellationToken);

            var serviceTimeRules = await _serviceTimeRuleReadRepository.GetByServiceIdAsync(booking.ServiceId, cancellationToken: cancellationToken);

            if (serviceTimeRules == null)
            {
                throw new BadRequestException("Chưa có suất khám");
            }
            var timeRule = serviceTimeRules.FirstOrDefault(x => x.DayOfWeek == (int)booking.Date.DayOfWeek);

            if (timeRule == null)
            {
                throw new BadRequestException("Ngày trong tuần không hợp lệ");
            }

            if (maxOrder == timeRule.MaxPatients)
            {
                throw new BadRequestException(_localizer["So luong da day"]);
            }

            booking.Status = BookingStatus.Confirmed;

            booking.Order = maxOrder + 1;

            _bookingWriteRepository.Update(booking);

            var notification = new Notification
            {
                Data = booking.Id.ToString(),
                IsUnread = true,
                Description = $"<p>Lịch khám <span class='n-bold'>{booking.Code}</span> vừa được xác nhận.</p>",
                Timestamp = DateTime.Now,
                Type = NotificationType.ConfirmBooking
            };

            var callbackWrapper = new CallbackWrapper();

            await _customerWriteRepository.AddNotificationForCustomerAsync(notification, booking.OwnerId, callbackWrapper, cancellationToken);

            await _bookingWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            await callbackWrapper.Callback();

            await _bookingWriteRepository.RemoveCacheWhenUpdateAsync(booking.Id, cancellationToken);

            await _bookingWriteRepository.ClearCacheAsync(booking, cancellationToken);

            await _socketService.ConfirmBooking(booking, cancellationToken);

            return Unit.Value;
        }
    }
}
