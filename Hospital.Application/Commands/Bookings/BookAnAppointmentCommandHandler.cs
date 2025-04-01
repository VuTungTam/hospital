using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.Symptoms;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    //Le tan booking
    public class BookAnAppointmentCommandHandler : BaseCommandHandler, IRequestHandler<BookAnAppointmentCommand, string>
    {
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
            IBookingWriteRepository bookingWriteRepository,
            IBookingReadRepository bookingReadRepository,
            IRedisCache redisCache,
            ISymptomReadRepository symptomReadRepository,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _symptomReadRepository = symptomReadRepository;
            _redisCache = redisCache;
        }

        public async Task<string> Handle(BookAnAppointmentCommand request, CancellationToken cancellationToken)
        {
            var booking = _mapper.Map<Booking>(request.Booking);

            var maxOrder = await _bookingReadRepository.GetMaxOrderAsync(booking.ServiceId, booking.Date, booking.TimeSlotId, cancellationToken);

            var maxSlot = await _serviceTimeRuleReadRepository.GetMaxSlotAsync(booking.ServiceId, booking.Date, cancellationToken);

            if (maxOrder == maxSlot)
            {
                throw new BadRequestException(_localizer["So luong da day"]);
            }

            var symptoms = await _symptomReadRepository.GetAsync(cancellationToken: cancellationToken);

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

            booking.Order = maxOrder + 1;

            booking.Status = BookingStatus.Completed;

            await _bookingWriteRepository.AddBookingCodeAsync(booking, cancellationToken);

            await _bookingWriteRepository.AddAsync(booking,cancellationToken);

            var cacheEntry = CacheManager.GetMaxOrderCacheEntry(booking.ServiceId, booking.Date, booking.TimeSlotId);

            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken: cancellationToken);

            await _redisCache.SetAsync(cacheEntry.Key, booking.Order, cancellationToken: cancellationToken);

            return booking.Id.ToString();
        }
    }
}
