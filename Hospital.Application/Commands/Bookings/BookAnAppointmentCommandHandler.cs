using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class BookAnAppointmentCommandHandler : BaseCommandHandler, IRequestHandler<BookAnAppointmentCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        private readonly ISymptomReadRepository _symptomReadRepository;
        private readonly IRedisCache _redisCache;
        public BookAnAppointmentCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IRedisCache redisCache,
            IBookingWriteRepository bookingWriteRepository,
            IBookingReadRepository bookingReadRepository,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository,
            ISymptomReadRepository symptomReadRepository
        ) : base(eventDispatcher, authService, localizer)
        {
            _mapper = mapper;
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _symptomReadRepository = symptomReadRepository;
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _redisCache = redisCache;
        }

        public async Task<string> Handle(BookAnAppointmentCommand request, CancellationToken cancellationToken)
        {
            var booking = _mapper.Map<Booking>(request.Booking);

            var oldOrder = await _bookingReadRepository.GetMaxOrderAsync(booking.ServiceId, booking.Date, 
                booking.ServiceStartTime, booking.ServiceEndTime, cancellationToken);

            if(oldOrder == await _serviceTimeRuleReadRepository.GetMaxSlotAsync(booking.ServiceId, booking.Date, cancellationToken))
            {
                throw new BadRequestException(_localizer["So luong da day"]);
            }


            booking.Status = BookingStatus.Waiting;

            await _bookingWriteRepository.AddWithCodeAsync(booking, cancellationToken);

            _bookingWriteRepository.Add(booking);

            await _bookingWriteRepository.SaveChangesAsync(cancellationToken);

            //foreach (var symptomId in request.Booking.Symptoms)
            //{
            //    if (!long.TryParse(symptomId, out var id) || id <= 0)
            //    {
            //        throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            //    }

            //    var symptom = await _symptomReadRepository.GetByIdAsync(id);
            //    if (symptom == null)
            //    {
            //        throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            //    }

            //    booking.BookingSymptom.Add(new BookingSymptom { BookingId = booking.Id, SymptomId = id });
            //}

            await _bookingWriteRepository.UpdateAsync(booking, cancellationToken: cancellationToken);

            return booking.Id.ToString();
        }
    }
}
