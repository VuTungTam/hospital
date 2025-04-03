using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Feedbacks;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Feedbacks;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Feedbacks
{
    public class AddFeedbackCommandHandler : BaseCommandHandler, IRequestHandler<AddFeedbackCommand, string>
    {
        private readonly IFeedbackReadRepository _feedbackReadRepository;
        private readonly IFeedbackWriteRepository _feedbackWriteRepository;
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly IHealthServiceWriteRepository _healthServiceWriteRepository;
        private readonly IRedisCache _redisCache;

        public AddFeedbackCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IFeedbackReadRepository feedbackReadRepository,
            IFeedbackWriteRepository feedbackWriteRepository,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            IHealthServiceReadRepository healthServiceReadRepository,
            IHealthServiceWriteRepository healthServiceWriteRepository,
            IRedisCache redisCache,
            IStringLocalizer<Resources> localizer, IMapper mapper
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _feedbackReadRepository = feedbackReadRepository;
            _feedbackWriteRepository = feedbackWriteRepository;
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
            _healthServiceWriteRepository = healthServiceWriteRepository;
            _redisCache = redisCache;
        }

        public async Task<string> Handle(AddFeedbackCommand request, CancellationToken cancellationToken)
        {
            var feedback = _mapper.Map<Feedback>(request.Dto);
            var booking = await _bookingReadRepository.GetByIdAsync(feedback.BookingId, cancellationToken: cancellationToken);
            if (booking == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            if (booking.IsFeedbacked)
            {
                throw new BadRequestException("booking đã feedback");
            }

            var service = await _healthServiceReadRepository.GetByIdAsync(booking.ServiceId, cancellationToken: cancellationToken);

            var serviceCacheEntry = new CacheEntry();

            if (service != null)
            {
                service.TotalFeedback += 1;

                service.TotalStars += feedback.Stars;

                service.StarPoint = service.TotalFeedback / service.TotalStars;

                serviceCacheEntry = await _healthServiceWriteRepository.SetBlockUpdateCacheAsync(service.Id, cancellationToken);

                _healthServiceWriteRepository.Update(service);
            }

            var bookingCacheEntry = await _bookingWriteRepository.SetBlockUpdateCacheAsync(service.Id, cancellationToken);

            feedback.BookingCode = booking.Code;

            _feedbackWriteRepository.Add(feedback);

            booking.IsFeedbacked = true;

            _bookingWriteRepository.Update(booking);

            await _feedbackWriteRepository.SaveChangesAsync(cancellationToken);

            await _feedbackWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);


            if (service != null)
            {
                await _redisCache.RemoveAsync(serviceCacheEntry.Key, cancellationToken);

                await _healthServiceWriteRepository.RemoveCacheWhenUpdateAsync(service.Id, cancellationToken);
            }

            await _redisCache.RemoveAsync(bookingCacheEntry.Key, cancellationToken);

            await _bookingWriteRepository.RemoveCacheWhenUpdateAsync(booking.Id, cancellationToken);

            await _feedbackWriteRepository.RemoveCacheWhenAddAsync(cancellationToken);

            return feedback.Id.ToString();
        }

    }

}
