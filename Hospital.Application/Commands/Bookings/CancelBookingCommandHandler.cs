using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.Bookings;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class CancelBookingCommandHandler : BaseCommandHandler, IRequestHandler<CancelBookingCommand>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IRedisCache _redisCache;
        public CancelBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            IRedisCache redisCache
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _redisCache = redisCache;
        }

        public async Task<Unit> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);

            var cancelBooking = await _bookingReadRepository.GetByIdAsync(
                request.Id, _bookingReadRepository.DefaultQueryOption, cancellationToken);

            if (cancelBooking == null)
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);

            switch (cancelBooking.Status)
            {
                case BookingStatus.Completed:
                case BookingStatus.Doing:
                    throw new BadRequestException(_localizer["Khong huy duoc lich "]);

                case BookingStatus.Waiting:
                    return await CancelBookingAsync(cancelBooking, cancellationToken);

                case BookingStatus.Confirmed:
                    return await CancelAndReorderAsync(cancelBooking, cancellationToken);

                default:
                    return Unit.Value;
            }
        }

        private async Task<Unit> CancelBookingAsync(Booking cancelBooking, CancellationToken cancellationToken)
        {
            cancelBooking.Status = BookingStatus.Cancel;
            await _bookingWriteRepository.UpdateAsync(cancelBooking, cancellationToken: cancellationToken);
            return Unit.Value;
        }

        private async Task<Unit> CancelAndReorderAsync(Booking cancelBooking, CancellationToken cancellationToken)
        {
            cancelBooking.Status = BookingStatus.Cancel;
            var canceledOrder = cancelBooking.Order;
            cancelBooking.Order = -1;

            _bookingWriteRepository.Update(cancelBooking);
            await _bookingWriteRepository.SaveChangesAsync(cancellationToken);

            var bookingsToUpdate = await GetBookingsToReorder(cancelBooking, canceledOrder, cancellationToken);
            if (bookingsToUpdate.Any())
            {
                foreach (var booking in bookingsToUpdate)
                {
                    booking.Order -= 1;
                    _bookingWriteRepository.Update(booking);
                }
                await _bookingWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
            }

            await _bookingWriteRepository.RemoveCacheWhenUpdateAsync(cancelBooking.Id, cancellationToken);
            return Unit.Value;
        }

        private async Task<List<Booking>> GetBookingsToReorder(Booking cancelBooking, int canceledOrder, CancellationToken cancellationToken)
        {
            var spec = new GetBookingsByDateSpecification(cancelBooking.Date)
                .And(new GetBookingsByServiceIdSpecification(cancelBooking.ServiceId))
                .And(new GetBookingsByTimeSlotSpecification(cancelBooking.TimeSlotId))
                .And(new GetBookingNextOrderSpecification(canceledOrder));

            var option = new QueryOption { IgnoreOwner = true };

            return await _bookingReadRepository.GetAsync(spec, option, cancellationToken);
        }
    }
}
