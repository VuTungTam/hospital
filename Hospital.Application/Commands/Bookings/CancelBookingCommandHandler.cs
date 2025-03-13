using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.Bookings;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Specifications.Interfaces;
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
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            IRedisCache redisCache
            ) : base(eventDispatcher, authService, localizer)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _redisCache = redisCache;
        }

        public async Task<Unit> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var cancelBooking = await _bookingReadRepository.GetByIdAsync(request.Id, _bookingReadRepository.DefaultQueryOption, cancellationToken);
            if (cancelBooking == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }
            if (cancelBooking.Status == BookingStatus.Completed)
            {
                throw new BadRequestException(_localizer["Khong huy duoc lich da thuc hien"]);
            }

            cancelBooking.Status = BookingStatus.Cancel;
            var cancelOrder = cancelBooking.Order;
            cancelBooking.Order = -1;

            _bookingWriteRepository.Update(cancelBooking);
            await _bookingWriteRepository.SaveChangesAsync(cancellationToken);

            var spec = new GetBookingsByDateSpecification(cancelBooking.Date)
                .And(new GetBookingsByServiceIdSpecification(cancelBooking.ServiceId))
                .And(new GetBookingsByTimeRangeSpecification(cancelBooking.ServiceStartTime, cancelBooking.ServiceEndTime))
                .And(new GetBookingNextOrderSpecification(cancelOrder));

            var option = new QueryOption
            {
                IgnoreOwner = true,
            };

            var bookings = await _bookingReadRepository.GetAsync(spec, option, cancellationToken);

            foreach (var booking in bookings)
            {
                if (booking.Order > cancelOrder)
                {
                    booking.Order = booking.Order - 1;
                    _bookingWriteRepository.Update(booking);
                }
            }

            await _bookingWriteRepository.SaveChangesAsync(cancellationToken);

            await _bookingWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return Unit.Value;
        }

    }
}
