using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Infrastructure.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthProfiles
{
    public class HealthProfileWriteRepository : WriteRepository<HealthProfile>, IHealthProfileWriteRepository
    {
        public HealthProfileWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task AddProfileAsync(HealthProfile healthProfile, CancellationToken cancellationToken = default)
        {
            var sequenceRepository = _serviceProvider.GetRequiredService<ISequenceRepository>();
            var table = new HealthProfile().GetTableName();
            var code = await sequenceRepository.GetSequenceAsync(table, cancellationToken);

            healthProfile.Code = code.ValueString;

            Add(healthProfile);

            await sequenceRepository.IncreaseValueAsync(table, cancellationToken);

            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
        }
    }
}
