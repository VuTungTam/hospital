﻿using Hospital.Application.Repositories.Interfaces.Branches;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Domain.Entities.Branches;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Branches
{
    public class BranchWriteRepository : WriteRepository<Branch>, IBranchWriteRepository
    {
        public BranchWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
    }
}