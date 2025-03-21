using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Feedbacks;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Entities.Bookings;
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
using Ocelot.Values;

namespace Hospital.Application.Commands.Feedbacks
{
    public class UpdateFeedbackCommandHandler : BaseCommandHandler, IRequestHandler<UpdateFeedbackCommand>
    {
        private readonly IFeedbackReadRepository _feedbackReadRepository;
        private readonly IFeedbackWriteRepository _feedbackWriteRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly IHealthServiceWriteRepository _healthServiceWriteRepository;
        private readonly IRedisCache _redisCache;
        public UpdateFeedbackCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService, 
            IStringLocalizer<Resources> localizer, 
            IMapper mapper,
            IFeedbackReadRepository feedbackReadRepository,
            IFeedbackWriteRepository feedbackWriteRepository,
            IHealthServiceReadRepository healthServiceReadRepository,
            IHealthServiceWriteRepository healthServiceWriteRepository,
            IRedisCache redisCache
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _feedbackReadRepository = feedbackReadRepository;
            _feedbackWriteRepository = feedbackWriteRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
            _healthServiceWriteRepository = healthServiceWriteRepository;
            _redisCache = redisCache;
        }

        public async Task<Unit> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.Feedback.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var feedback = await _feedbackReadRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
            if (feedback == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            if (feedback.ModifiedAt != null)
            {
                throw new BadRequestException(_localizer["Feedback đã được chỉnh sửa"]);
            }
            if ((DateTime.Now - feedback.CreatedAt).Days > 30 )
            {
                throw new BadRequestException(_localizer["Không được chỉnh sửa feedback quá 30 ngày"]);
            }
            var entity = _mapper.Map<Feedback>(request.Feedback);

            var starChange = (entity.Stars != feedback.Stars);

            var starChangeCacheEntry = new CacheEntry();

            if (starChange) {
                var service = await _healthServiceReadRepository.GetByIdAsync(feedback.Booking.ServiceId, cancellationToken: cancellationToken);

                if (service != null)
                {
                    service.TotalStars = service.TotalStars - feedback.Stars + entity.Stars;

                    service.StarPoint = (float)service.TotalStars / service.TotalFeedback;

                    starChangeCacheEntry = await _healthServiceWriteRepository.SetBlockUpdateCacheAsync(service.Id, cancellationToken);

                    _healthServiceWriteRepository.Update(service);
                }
            }

            await _feedbackWriteRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            if (starChange)
            {
                await _redisCache.RemoveAsync(starChangeCacheEntry.Key, cancellationToken);

                await _healthServiceWriteRepository.RemoveCacheWhenUpdateAsync(feedback.Booking.ServiceId, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
