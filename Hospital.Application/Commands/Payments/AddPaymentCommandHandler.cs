using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Payments;
using Hospital.Domain.Entities.Payments;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Payments
{
    public class AddPaymentCommandHandler : BaseCommandHandler, IRequestHandler<AddPaymentCommand, string>
    {
        public readonly IPaymentWriteRepository _paymentWriteRepository;
        public readonly IPaymentReadRepository _paymentReadRepository;
        public readonly ISequenceRepository _sequenceRepository;
        public readonly IBookingReadRepository _bookingReadRepository;
        public AddPaymentCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IPaymentWriteRepository paymentWriteRepository,
            IPaymentReadRepository paymentReadRepository,
            ISequenceRepository sequenceRepository,
            IBookingReadRepository bookingReadRepository,
            IMapper mapper
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _paymentWriteRepository = paymentWriteRepository;
            _paymentReadRepository = paymentReadRepository;
            _sequenceRepository = sequenceRepository;
            _bookingReadRepository = bookingReadRepository;
        }

        public async Task<string> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
        {
            var option = new QueryOption
            {
                IgnoreDoctor = true,
                IgnoreFacility = true,
                IgnoreZone = true,
            };

            var booking = await _bookingReadRepository.GetByIdAsync(long.Parse(request.Dto.BookingId), option, cancellationToken: cancellationToken);

            if (booking == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var newPayment = _mapper.Map<Payment>(request.Dto);

            newPayment.FacilityId = booking.FacilityId;

            await _paymentWriteRepository.AddAsync(newPayment, cancellationToken);

            return newPayment.Id.ToString();
        }
    }
}
