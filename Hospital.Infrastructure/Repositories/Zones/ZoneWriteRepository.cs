using AutoMapper;
using Hospital.Application.Dtos.Zones;
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
        private readonly IMapper _mapper;
        public ZoneWriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IRedisCache redisCache
            ) : base(serviceProvider, localizer, redisCache)
        {
            _mapper = mapper;
        }

        public Task RemoveSpecialtiesAsync(List<ZoneSpecialty> zoneSpecialties, CancellationToken cancellationToken)
        {
            _dbContext.ZoneSpecialties.RemoveRange(zoneSpecialties);

            return Task.CompletedTask;
        }

        public async Task UpdateZoneAsync(Zone oldZone, ZoneDto newZone, CancellationToken cancellationToken)
        {
            oldZone.NameVn = newZone.NameVn;
            oldZone.NameEn = newZone.NameEn;
            oldZone.LocationVn = newZone.LocationVn;
            oldZone.LocationEn = newZone.LocationEn;

            var oldSpecs = oldZone.ZoneSpecialties.Select(x => x.SpecialtyId.ToString()).ToList();
            var newSpecs = newZone.SpecialtyIds;
            var delSpecs = oldZone.ZoneSpecialties.Where(s => !newSpecs.Contains(s.SpecialtyId.ToString())).ToList();
            var addSpecs = newSpecs.Except(oldSpecs).ToList();

            _dbContext.ZoneSpecialties.RemoveRange(delSpecs);
            foreach (var spec in addSpecs)
            {
                _dbContext.ZoneSpecialties.Add(new ZoneSpecialty
                {
                    Id = AuthUtility.GenerateSnowflakeId(),
                    ZoneId = oldZone.Id,
                    SpecialtyId = long.Parse(spec)
                });
            }

            //_mapper.Map(newZone, oldZone);
            await UpdateAsync(oldZone, cancellationToken: cancellationToken);
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
