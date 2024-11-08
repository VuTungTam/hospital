using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Bookings;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class UpdateBookingCommandHandler : BaseCommandHandler, IRequestHandler<UpdateBookingCommand>
    {
        private readonly IMapper _mapper;
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;

        public UpdateBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository
        ) : base(eventDispatcher, authService, localizer)
        {
            _mapper = mapper;
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
        }

        public async Task<Unit> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.Booking.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var booking = await _bookingReadRepository.GetByIdAsync(id, ignoreOwner: true, cancellationToken: cancellationToken);
            if (booking == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            var entity = _mapper.Map<Booking>(request.Booking);

            await _bookingWriteRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
