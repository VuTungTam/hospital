using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class DeleteBookingsCommandHandler : BaseCommandHandler, IRequestHandler<DeleteBookingsCommand>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;

        public DeleteBookingsCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository
        ) : base(eventDispatcher, authService, localizer)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
        }

        public async Task<Unit> Handle(DeleteBookingsCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || request.Ids.Exists(id => id <= 0))
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var bookings = await _bookingReadRepository.GetByIdsAsync(request.Ids, _bookingReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
            if (bookings.Any())
            {
                await _bookingWriteRepository.DeleteAsync(bookings, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
