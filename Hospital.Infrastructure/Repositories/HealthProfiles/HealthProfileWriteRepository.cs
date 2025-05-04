using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Infrastructure.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthProfiles
{
    public class HealthProfileWriteRepository : WriteRepository<HealthProfile>, IHealthProfileWriteRepository
    {
        private readonly ILocationReadRepository _locationReadRepository;
        public HealthProfileWriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            ILocationReadRepository locationReadRepository,
            IRedisCache redisCache
            ) : base(serviceProvider, localizer, redisCache)
        {
            _locationReadRepository = locationReadRepository;
        }

        public async Task AddProfileAsync(HealthProfile healthProfile, CancellationToken cancellationToken = default)
        {
            var sequenceRepository = _serviceProvider.GetRequiredService<ISequenceRepository>();
            var table = new HealthProfile().GetTableName();
            var code = await sequenceRepository.GetSequenceAsync(table, cancellationToken);

            healthProfile.Code = code.ValueString;

            healthProfile.Pname = await _locationReadRepository.GetNameByIdAsync(healthProfile.Pid, "province", cancellationToken);
            healthProfile.Dname = await _locationReadRepository.GetNameByIdAsync(healthProfile.Did, "district", cancellationToken);
            healthProfile.Wname = await _locationReadRepository.GetNameByIdAsync(healthProfile.Wid, "ward", cancellationToken);

            Add(healthProfile);

            await sequenceRepository.IncreaseValueAsync(table, cancellationToken);

            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
        }
    }
}
