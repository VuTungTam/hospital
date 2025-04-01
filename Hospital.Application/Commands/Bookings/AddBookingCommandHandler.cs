using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class AddBookingCommandHandler : BaseCommandHandler, IRequestHandler<AddBookingCommand, string>
    {
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly ISymptomReadRepository _symptomReadRepository;
        public AddBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingWriteRepository bookingWriteRepository,
            ISymptomReadRepository symptomReadRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingWriteRepository = bookingWriteRepository;
            _symptomReadRepository = symptomReadRepository;
        }

        public async Task<string> Handle(AddBookingCommand request, CancellationToken cancellationToken)
        {
            var symptoms = await _symptomReadRepository.GetAsync(cancellationToken: cancellationToken);

            var booking = _mapper.Map<Booking>(request.Booking);

            booking.Status = BookingStatus.Waiting;

            booking.BookingSymptoms = new();

            foreach (var symptom in request.Booking.Symptoms)
            {
                var symptomDb = symptoms.First(x => x.Id + "" == symptom.Id);

                booking.BookingSymptoms.Add(new BookingSymptom
                {
                    Id = AuthUtility.GenerateSnowflakeId(),
                    SymptomId = symptomDb.Id,
                });
            }

            await _bookingWriteRepository.AddBookingCodeAsync(booking, cancellationToken);

            await _bookingWriteRepository.AddAsync(booking, cancellationToken);

            return booking.Id.ToString();
        }
    }
}
