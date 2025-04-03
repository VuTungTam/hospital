using Hospital.Application.Repositories.Interfaces.Zones;
using Hospital.Domain.Entities.Zones;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Zones
{
    public class ZoneWriteRepository : WriteRepository<Zone>, IZoneWriteRepository
    {
        public ZoneWriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer, 
            IRedisCache redisCache
            ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public Task RemoveSpecialtiesAsync(List<ZoneSpecialty> zoneSpecialties, CancellationToken cancellationToken)
        {
            _dbContext.ZoneSpecialties.RemoveRange(zoneSpecialties);

            return Task.CompletedTask;
        }

        public async Task UpdateZoneSpecialtiesAsync(long zoneId, IEnumerable<long> speIds, CancellationToken cancellationToken)
        {
            var sql = $"DELETE FROM {new ZoneSpecialty().GetTableName()} WHERE {nameof(ZoneSpecialty.ZoneId)} = {zoneId}; ";
            foreach (var speId in speIds)
            {
                sql += $"INSERT INTO {new ZoneSpecialty().GetTableName()}(Id, ZoneId, {nameof(ZoneSpecialty.SpecialtyId)}) VALUES ({AuthUtility.GenerateSnowflakeId()},{zoneId}, {speId}); ";
            }

            await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken);
        }

    }
}
