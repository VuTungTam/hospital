using AutoMapper;
using Hospital.Application.Commands.Feedbacks;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.CancelReasons;
using Hospital.Domain.Entities.CancelReasons;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.CancelReasons
{
    public class AddCancelReasonCommandHandler : BaseCommandHandler, IRequestHandler<AddCancelReasonCommand, string>
    {
        private readonly ICancelReasonWriteRepository _cancelReasonWriteRepository;
        private readonly IBookingReadRepository _bookingReadRepository;

        public AddCancelReasonCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            ICancelReasonWriteRepository cancelReasonWriteRepository,
            IBookingReadRepository bookingReadRepository,
            IStringLocalizer<Resources> localizer, IMapper mapper
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _cancelReasonWriteRepository = cancelReasonWriteRepository;
            _bookingReadRepository = bookingReadRepository;
        }

        public async Task<string> Handle(AddCancelReasonCommand request, CancellationToken cancellationToken)
        {
            var cancelReason = _mapper.Map<CancelReason>(request.Dto);
            var booking = await _bookingReadRepository.GetByIdAsync(cancelReason.BookingId, cancellationToken: cancellationToken);

            if (booking == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            if (booking.Status == BookingStatus.None || booking.Status == BookingStatus.Cancel || booking.Status == BookingStatus.Completed)
            {
                throw new BadRequestException("booking đã hủy");
            }

            var reason = _mapper.Map<CancelReason>(request.Dto);

            await _cancelReasonWriteRepository.AddAsync(reason, cancellationToken);

            return reason.Id.ToString();
        }
    }

}
