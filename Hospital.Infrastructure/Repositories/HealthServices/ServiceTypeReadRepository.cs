using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthServices
{
    public class ServiceTypeReadRepository : ReadRepository<ServiceType>, IServiceTypeReadRepository
    {
        public ServiceTypeReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<ServiceType> GetTypeBySlugAndLangsAsync(string slug, List<string> langs, CancellationToken cancellationToken)
        {
            var cacheEntry = AppCacheManager.GetServiceTypeBySlugAndLangsCacheEntry(slug, langs);
            var data = await _redisCache.GetAsync<ServiceType>(cacheEntry.Key, cancellationToken);
            if (data == null)
            {
                var whereClause = "WHERE Slug = {0}";

                var columns = string.Join(", ",
                    "Slug", "Id", "Logo", "Image",
                    langs.Contains("vi-VN") ? "NameVn" : "'' AS NameVn",
                    langs.Contains("vi-VN") ? "DescriptionVn" : "'' AS DescriptionVn",
                    langs.Contains("en-US") ? "NameEn" : "'' AS NameEn",
                    langs.Contains("en-US") ? "DescriptionEn" : "'' AS DescriptionEn"
                );

                var sql = $@"SELECT {columns} FROM {new ServiceType().GetTableName()} {whereClause}";

                data = await _dbSet.FromSqlRaw(sql, slug)
                                   .IgnoreQueryFilters()
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(cancellationToken);
            }

            if (data == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataDoesNotExistOrWasDeleted"]);
            }

            return data;
        }
    }
}
