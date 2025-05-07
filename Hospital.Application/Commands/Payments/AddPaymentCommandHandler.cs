using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Payments;
using Hospital.Domain.Entities.Payments;
using Hospital.Domain.Specifications.Payments;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
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
        public readonly ILocationReadRepository _locationReadRepository;
        public readonly ISequenceRepository _sequenceRepository;
        public readonly IBookingReadRepository _bookingReadRepository;
        public AddPaymentCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IPaymentWriteRepository paymentWriteRepository,
            IPaymentReadRepository paymentReadRepository,
            ISequenceRepository sequenceRepository,
            ILocationReadRepository locationReadRepository,
            IBookingReadRepository bookingReadRepository,
            IMapper mapper
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _paymentWriteRepository = paymentWriteRepository;
            _paymentReadRepository = paymentReadRepository;
            _locationReadRepository = locationReadRepository;
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
                IgnoreOwner = true,
            };

            var booking = await _bookingReadRepository.GetByIdAsync(request.Dto.BookingId, option, cancellationToken: cancellationToken);

            if (booking == null)
            {
                throw new BadRequestException("Không thấy bản ghi");
            }

            var spec = new GetPaymentsByBookingIdSpecification(request.Dto.BookingId);

            var payments = await _paymentReadRepository.GetAsync(option, cancellationToken);

            //payments = payments.Where(x => x.BookingId == request.Dto.BookingId)

            var now = DateTime.Now;

            foreach (var p in payments)
            {
                if (p.IsPaid)
                    throw new BadRequestException(_localizer["Booking_Already_Paid"]);

                if (!p.IsPaid && (p.ExpiredAt == null || p.ExpiredAt > now))
                    throw new BadRequestException(_localizer["Payment_Still_Pending"]);
            }


            var newPayment = _mapper.Map<Payment>(request.Dto);

            await _paymentWriteRepository.AddAsync(newPayment, cancellationToken);

            return newPayment.Id.ToString();
        }
    }
}
