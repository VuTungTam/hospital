using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.HealthFacilities;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthFacilities
{
    public class HealthFacilityReadRepository : ReadRepository<HealthFacility>, IHealthFacilityReadRepository
    {
        public HealthFacilityReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task<PaginationResult<HealthFacility>> GetPagingWithFilterAsync(Pagination pagination, long facilityTypeId = 0, HealthFacilityStatus status = default, CancellationToken cancellationToken = default)
        {
            ISpecification<HealthFacility> spec = new GetHealthFacilitiesByStatusSpecification(status);
            if (facilityTypeId > 0)
            {
                spec = spec.And(new GetHealthFacilitiesByTypeIdSpecification(facilityTypeId));
            }
            QueryOption option = new QueryOption();
            var guardExpression = GuardDataAccess(spec, option).GetExpression();
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(guardExpression)
                         .OrderByDescending(x => x.ModifiedAt ?? x.CreatedAt);

            var data = await query.BuildLimit(pagination.Offset, pagination.Size)
                                  .ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken);

            return new PaginationResult<HealthFacility>(data, count);
        }

        public async Task<HealthFacility> GetBySlugAndLangsAsync(string slug, List<string> langs, CancellationToken cancellationToken)
        {
            var cacheEntry = AppCacheManager.GetServiceBySlugAndLangsCacheEntry(slug, langs);
            var data = await _redisCache.GetAsync<HealthFacility>(cacheEntry.Key, cancellationToken);
            if (data == null)
            {
                var whereClause = "WHERE Slug = {0} AND IsDeleted = 0";

                var columns = string.Join(", ",
                    "Slug", "Id", "Image", "Address", "Status", "Did", "Dname", "Email",
                    "Latitude", "Longitude", "MapURL", "Phone", "Pid", "Pname",
                    "StarPoint", "TotalFeedback", "TotalStars", "Website", "Wid", "Wname",
                    "CreatedBy", "CreatedAt", "ModifiedBy", "ModifiedAt", "IsDeleted", "DeletedAt", "DeletedBy",
                    langs.Contains("vi-VN") ? "NameVn" : "'' AS NameVn",
                    langs.Contains("vi-VN") ? "DescriptionVn" : "'' AS DescriptionVn",
                    langs.Contains("vi-VN") ? "SummaryVn" : "'' AS SummaryVn",
                    langs.Contains("en-US") ? "NameEn" : "'' AS NameEn",
                    langs.Contains("en-US") ? "DescriptionEn" : "'' AS DescriptionEn",
                    langs.Contains("en-US") ? "SummaryEn" : "'' AS SummaryEn"
                );

                var sql = $@"SELECT {columns} FROM {new HealthFacility().GetTableName()} {whereClause}";

                data = await _dbSet.FromSqlRaw(sql, slug)
                                   .IgnoreQueryFilters()
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(cancellationToken);

                if (data != null)
                {
                    data.Images = await _dbContext.Images
                            .Where(img => img.FacilityId == data.Id && !img.IsDeleted)
                            .AsNoTracking()
                            .ToListAsync(cancellationToken);
                    await _redisCache.SetAsync(cacheEntry.Key, data, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
                }
            }

            if (data == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataDoesNotExistOrWasDeleted"]);
            }

            return data;
        }

        public async Task<PaginationResult<HealthFacility>> GetNamePaginationAsync(Pagination pagination, CancellationToken cancellationToken = default)
        {
            ISpecification<HealthFacility> spec = new GetHealthFacilitiesByStatusSpecification(HealthFacilityStatus.InActive);

            var specWithGuard = GuardDataAccess(spec);
            var expression = specWithGuard.GetExpression();

            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination)
                         .Where(expression);

            var countFactory = () => query.CountAsync(cancellationToken); 

            var dataFactory = () => query
                .OrderBy(x => x.NameVn)
                .Select(x => new HealthFacility
                {
                    Id = x.Id,
                    NameVn = x.NameVn,
                    NameEn = x.NameEn
                })
                .BuildLimit(pagination.Offset, pagination.Size)
                .ToListAsync(cancellationToken);

            var cacheEntry = CacheManager.GetPaginationCacheEntry<HealthFacility>(pagination, 0, 0);
            var valueFactory = async () => new PaginationResult<HealthFacility>(await dataFactory(), await countFactory());
            return await _redisCache.GetOrSetAsync(cacheEntry.Key, valueFactory, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
        }
    }
}
