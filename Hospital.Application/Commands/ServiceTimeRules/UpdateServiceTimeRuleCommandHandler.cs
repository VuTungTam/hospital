using AutoMapper;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Entities.TimeSlots;
using Hospital.Domain.Specifications.TimeSlots;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    public class UpdateServiceTimeRuleCommandHandler : BaseCommandHandler, IRequestHandler<UpdateServiceTimeRuleCommand>
    {
        private readonly IRedisCache _redisCache;
        private readonly ITimeSlotReadRepository _timeSlotReadRepository;
        private readonly ITimeSlotWriteRepository _timeSlotWriteRepository;
        private readonly IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        private readonly IServiceTimeRuleWriteRepository _serviceTimeRuleWriteRepository;
        public UpdateServiceTimeRuleCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IRedisCache redisCache,
            ITimeSlotReadRepository timeSlotReadRepository,
            ITimeSlotWriteRepository timeSlotWriteRepository,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository,
            IServiceTimeRuleWriteRepository serviceTimeRuleWriteRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _serviceTimeRuleWriteRepository = serviceTimeRuleWriteRepository;
            _timeSlotReadRepository = timeSlotReadRepository;
            _timeSlotWriteRepository = timeSlotWriteRepository;
            _redisCache = redisCache;
        }

        public async Task<Unit> Handle(UpdateServiceTimeRuleCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.Dto.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var timeRule = await _serviceTimeRuleReadRepository.GetByIdAsync(id, _serviceTimeRuleReadRepository.DefaultQueryOption, cancellationToken);
            if (timeRule == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataDoesNotExistOrWasDeleted"]);
            }

            var entity = _mapper.Map<ServiceTimeRule>(request.Dto);

            var cacheEntry = await _serviceTimeRuleWriteRepository.SetBlockUpdateCacheAsync(timeRule.Id, cancellationToken);

            _serviceTimeRuleWriteRepository.Update(entity);

            var newTimeSlots = _serviceTimeRuleWriteRepository.GenerateTimeSlots(entity);

            var oldTimeSlots = await _timeSlotReadRepository.GetByTimeRuleIdAsync(timeRule.Id, cancellationToken: cancellationToken);

            var comparer = new TimeSlotEqualityComparer();

            var oldTimeSlotSet = new HashSet<TimeSlot>(oldTimeSlots, comparer);
            var newTimeSlotSet = new HashSet<TimeSlot>(newTimeSlots, comparer);

            var deleteTimeSlots = oldTimeSlots.Where(slot => !newTimeSlotSet.Contains(slot, comparer)).ToList();

            var addTimeSlots = newTimeSlots
                .Where(slot => !oldTimeSlotSet.Contains(slot, comparer))
                .ToList();

            _timeSlotWriteRepository.Delete(deleteTimeSlots);

            _timeSlotWriteRepository.AddRange(addTimeSlots);

            await _timeSlotWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken);

            await _serviceTimeRuleWriteRepository.RemoveCacheWhenUpdateAsync(timeRule.Id, cancellationToken);

            var cacheEntry2 = CacheManager.GetTimeRulesEntry(timeRule.ServiceId);

            await _redisCache.RemoveAsync(cacheEntry2.Key, cancellationToken);

            await _timeSlotWriteRepository.RemoveCacheWhenAddAsync(null, cancellationToken);

            var cacheEntry3 = CacheManager.GetTimeSlotsEntry(timeRule.Id);

            await _redisCache.RemoveAsync(cacheEntry3.Key, cancellationToken);

            return Unit.Value;
        }

    }
}
