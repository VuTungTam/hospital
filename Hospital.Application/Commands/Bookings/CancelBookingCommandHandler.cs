﻿using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.CancelReasons;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Application.Services.Interfaces.Sockets;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.CancelReasons;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Enums;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class CancelBookingCommandHandler : BaseCommandHandler, IRequestHandler<CancelBookingCommand>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IEmployeeWriteRepository _employeeWriteRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        private readonly ICancelReasonWriteRepository _cancelReasonWriteRepository;
        private readonly IExecutionContext _executionContext;
        private readonly ISocketService _socketService;
        public CancelBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            ICustomerWriteRepository customerWriteRepository,
            IEmployeeWriteRepository employeeWriteRepository,
            ICancelReasonWriteRepository cancelReasonWriteRepository,
            ISocketService socketService,
            IExecutionContext executionContext
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _employeeWriteRepository = employeeWriteRepository;
            _customerWriteRepository = customerWriteRepository;
            _socketService = socketService;
            _executionContext = executionContext;
            _cancelReasonWriteRepository = cancelReasonWriteRepository;
        }

        public async Task<Unit> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            if (request.Model.BookingId <= 0)
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);

            var cancelBooking = await _bookingReadRepository.GetByIdAsync(
                request.Model.BookingId, _bookingReadRepository.DefaultQueryOption, cancellationToken);

            if (cancelBooking == null)
                throw new BadRequestException(_localizer["CommonMessage.DataDoesNotExistOrWasDeleted"]);
            var bookingsToUpdate = new List<Booking>();
            var removeNextCache = false;
            switch (cancelBooking.Status)
            {
                case BookingStatus.Completed:
                case BookingStatus.Doing:
                    throw new BadRequestException(_localizer["Khong huy duoc lich "]);

                case BookingStatus.Waiting:
                    await CancelBookingAsync(cancelBooking, cancellationToken);
                    break;
                case BookingStatus.Confirmed:
                    bookingsToUpdate = await CancelAndReorderAsync(cancelBooking, cancellationToken);
                    removeNextCache = true;
                    break;
                default:
                    break;

            }

            AddCancelReason(cancelBooking, request.Model.Reason);

            await SendNotification(cancelBooking, cancellationToken);

            await _bookingWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            await _bookingWriteRepository.RemoveCacheWhenUpdateAsync(cancelBooking.Id, cancellationToken);

            if (removeNextCache)
            {
                foreach (var booking in bookingsToUpdate)
                {
                    await _bookingWriteRepository.RemoveOneRecordCacheAsync(booking.Id, cancellationToken);
                }
            }

            await _bookingWriteRepository.ClearCacheAsync(cancelBooking, cancellationToken);

            await SendSignalR(cancelBooking, cancellationToken);

            return Unit.Value;
        }

        private async Task<Unit> CancelBookingAsync(Booking cancelBooking, CancellationToken cancellationToken)
        {
            cancelBooking.Status = BookingStatus.Cancel;

            _bookingWriteRepository.Update(cancelBooking);

            await _bookingWriteRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task<List<Booking>> CancelAndReorderAsync(Booking cancelBooking, CancellationToken cancellationToken)
        {
            var bookingsToUpdate = await _bookingReadRepository.GetNextBookings(cancelBooking, cancellationToken);
            cancelBooking.Status = BookingStatus.Cancel;
            cancelBooking.Order = -1;

            _bookingWriteRepository.Update(cancelBooking);

            await _bookingWriteRepository.SaveChangesAsync(cancellationToken);

            if (bookingsToUpdate.Any())
            {
                foreach (var booking in bookingsToUpdate)
                {
                    booking.Order -= 1;
                    _bookingWriteRepository.Update(booking);
                }

            }
            return bookingsToUpdate;
        }

        private async Task<Unit> SendCustomerNotification(Booking booking, CallbackWrapper callbackWrapper, CancellationToken cancellationToken)
        {

            var notification = new Notification
            {
                Data = booking.Id.ToString(),
                IsUnread = true,
                Description = $"<p>Lịch khám <span class='n-bold'>{booking.Code}</span> vừa bị hủy.</p>",
                Timestamp = DateTime.Now,
                Type = NotificationType.ConfirmBooking
            };

            await _customerWriteRepository.AddNotificationForCustomerAsync(notification, booking.OwnerId, callbackWrapper, cancellationToken);

            return Unit.Value;
        }

        private async Task<Unit> SendEmployeeNotification(Booking booking, CallbackWrapper callbackWrapper, CancellationToken cancellationToken)
        {

            var notification = new Notification
            {
                Data = booking.Id.ToString(),
                IsUnread = true,
                Description = $"<p>Khách hàng vừa hủy lịch khám <span class='n-bold'>{booking.Code}</span>.</p>",
                Timestamp = DateTime.Now,
                Type = NotificationType.ConfirmBooking
            };

            await _employeeWriteRepository.AddNotificationForEmployeeAsync(notification, booking.FacilityId, booking.ZoneId, callbackWrapper, cancellationToken);

            return Unit.Value;
        }

        private async Task<Unit> SendSignalR(Booking booking, CancellationToken cancellationToken)
        {
            if (booking.OwnerId == _executionContext.Identity)
            {
                await _socketService.CustomerCancelBooking(booking, cancellationToken);
            }
            else
            {
                await _socketService.EmployeeCancelBooking(booking, cancellationToken);
            }
            return Unit.Value;
        }

        private async Task SendNotification(Booking booking, CancellationToken cancellationToken)
        {
            var callbackWrapper = new CallbackWrapper();

            if (booking.OwnerId == _executionContext.Identity)
            {
                await SendEmployeeNotification(booking, callbackWrapper, cancellationToken);
            }
            else
            {
                await SendCustomerNotification(booking, callbackWrapper, cancellationToken);
            }

            await callbackWrapper.Callback();

        }

        private void AddCancelReason(Booking booking, string reasonMsg)
        {
            var reason = new CancelReason
            {
                Id = AuthUtility.GenerateSnowflakeId(),
                Reason = reasonMsg,
                CancelType = (booking.OwnerId == _executionContext.Identity) ? CancelType.Customer : CancelType.Coordinator,
                BookingId = booking.Id,
            };

            _cancelReasonWriteRepository.Add(reason);
        }
    }
}
