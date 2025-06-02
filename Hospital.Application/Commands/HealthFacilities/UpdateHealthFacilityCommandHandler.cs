using AutoMapper;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealthFacilities
{
    public class UpdateHealthFacilityCommandHandler : BaseCommandHandler, IRequestHandler<UpdateHealthFacilityCommand>
    {
        private readonly IHealthFacilityWriteRepository _healthFacilityWriteRepository;
        private readonly IHealthFacilityReadRepository _healthFacilitReadRepository;
        private readonly IRedisCache _redisCache;
        public UpdateHealthFacilityCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IRedisCache redisCache,
            IHealthFacilityWriteRepository healthFacilityWriteRepository,
            IHealthFacilityReadRepository healthFacilitReadRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _healthFacilityWriteRepository = healthFacilityWriteRepository;
            _healthFacilitReadRepository = healthFacilitReadRepository;
            _redisCache = redisCache;
        }


        public async Task<Unit> Handle(UpdateHealthFacilityCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.HealthFacility.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var option = new QueryOption
            {
                Includes = new string[] { nameof(HealthFacility.Images), nameof(HealthFacility.FacilitySpecialties), nameof(HealthFacility.FacilityTypeMappings) }
            };

            var facility = await _healthFacilitReadRepository.GetByIdAsync(id, option, cancellationToken: cancellationToken);

            if (facility == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataDoesNotExistOrWasDeleted"]);
            }

            await _healthFacilityWriteRepository.UpdateFacilityAsync(facility, request.HealthFacility, cancellationToken: cancellationToken);

            await _healthFacilityWriteRepository.RemoveCacheWhenUpdateAsync(facility.Id, cancellationToken);

            var cacheEntry4 = AppCacheManager.GetFacilityBySlugAndLangsCacheEntry(facility.Slug, new List<string> { "vi-VN", "en-US" });
            var cacheEntry5 = AppCacheManager.GetFacilityBySlugAndLangsCacheEntry(facility.Slug, new List<string> { "vi-VN" });
            var cacheEntry6 = AppCacheManager.GetFacilityBySlugAndLangsCacheEntry(facility.Slug, new List<string> { "en-US" });

            await Task.WhenAll(
                _redisCache.RemoveAsync(cacheEntry4.Key, cancellationToken),
                _redisCache.RemoveAsync(cacheEntry5.Key, cancellationToken),
                _redisCache.RemoveAsync(cacheEntry6.Key, cancellationToken)
            );

            return Unit.Value;
        }
    }
}
