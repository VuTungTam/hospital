﻿using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthFacilities
{
    public class HealthFacilityWriteRepository : WriteRepository<HealthFacility>, IHealthFacilityWriteRepository
    {
        public HealthFacilityWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}