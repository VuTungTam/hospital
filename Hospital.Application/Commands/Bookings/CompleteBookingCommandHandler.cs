﻿using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Services.Interfaces.Sockets;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Date;
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
    public class CompleteBookingCommandHandler : BaseCommandHandler, IRequestHandler<CompleteBookingCommand>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        private readonly IRedisCache _redisCache;
        private readonly IDateService _dateService;
        private readonly ISocketService _socketService;
        public CompleteBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository,
            ICustomerWriteRepository customerWriteRepository,
            IRedisCache redisCache,
            IDateService dateService,
            ISocketService socketService
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _redisCache = redisCache;
            _dateService = dateService;
            _socketService = socketService;
            _customerWriteRepository = customerWriteRepository;
        }

        public async Task<Unit> Handle(CompleteBookingCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var option = new QueryOption
            {
                IgnoreOwner = true
            };

            var booking = await _bookingReadRepository.GetByIdAsync(request.Id, option, cancellationToken: cancellationToken);
            if (booking == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataDoesNotExistOrWasDeleted"]);
            }

            if (booking.Status != BookingStatus.Doing)
            {
                throw new BadRequestException(_localizer["Booking.IsNotDoing"]);
            }

            booking.EndBooking = _dateService.GetClientTime().TimeOfDay;

            booking.Status = BookingStatus.Completed;

            var notification = new Notification
            {
                Data = booking.Id.ToString(),
                IsUnread = true,
                Description = $"<p>Lịch khám <span class='n-bold'>{booking.Code}</span> vừa được hoàn thành, cảm ơn bạn đã sử dụng dịch vụ của chúng tôi</p>",
                Timestamp = DateTime.Now,
                Type = NotificationType.ConfirmBooking
            };

            var callbackWrapper = new CallbackWrapper();

            await _customerWriteRepository.AddNotificationForCustomerAsync(notification, booking.OwnerId, callbackWrapper, cancellationToken);

            await _bookingWriteRepository.UpdateAsync(booking, cancellationToken: cancellationToken);

            var cacheEntry = CacheManager.GetCurrentOrderCacheEntry(booking.ServiceId, booking.Date, booking.TimeSlotId);

            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken: cancellationToken);

            //await _socketService.CompleteBooking(booking, cancellationToken);

            return Unit.Value;
        }
    }
}
