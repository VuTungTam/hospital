using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.Specialties;
using Hospital.Infrastructure.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Attributes;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Runtime.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthFacilities
{
    public class HealthFacilityWriteRepository : WriteRepository<HealthFacility>, IHealthFacilityWriteRepository
    {
        public HealthFacilityWriteRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }
        public async Task RemoveFacilitySpecialtyAsync(FacilitySpecialty facilitySpecialty, CancellationToken cancellationToken)
        {
            var dateService = _serviceProvider.GetRequiredService<IDateService>();
            var clientTime = dateService.GetClientTime();
            var tableName = new FacilitySpecialty().GetTableName();
            var sql = $"UPDATE {tableName} SET Deleted = @ClientTime WHERE Id = @FacilitySpecialtyId";
            await _dbContext.Database.ExecuteSqlRawAsync(sql,
                new[]
                {
                new SqlParameter("@ClientTime", clientTime),
                new SqlParameter("@FacilitySpecialtyId", facilitySpecialty.Id)
                },
                cancellationToken: cancellationToken);
            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
        }
    }
}
