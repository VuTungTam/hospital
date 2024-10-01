using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Databases.Dapper;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Microsoft.Extensions.Localization;


namespace Hospital.Infrastructure.Repositories.Locations
{
    public class LocationReadRepository : ReadRepository<BaseLocation>, ILocationReadRepository
    {
        private readonly IDbConnection _dbConnection;
        public LocationReadRepository(
            IServiceProvider serviceProvider, 
            IStringLocalizer<Resources> localizer,
            IDbConnection dbConnection,
            IRedisCache redisCache
            ) : base(serviceProvider, localizer, redisCache)
        {
            _dbConnection = dbConnection;
        }
        
        public async Task<PagingResult<District>> GetDistrictsPagingAsync(int provinceId, Pagination pagination, CancellationToken cancellationToken)
        {
            var sql = $"SELECT * FROM {new District().GetTableName()} WHERE 1=1";
            var sqlCount = sql.Replace("*", "COUNT(*)");

            if (provinceId > 0)
            {
                sql += $" AND ProvinceId = {provinceId}";
                sqlCount += $" AND ProvinceId = {provinceId}";
            }

            if (!string.IsNullOrEmpty(pagination.Search))
            {
                var whereClause = $" AND ({nameof(District.Name)} LIKE '%' + @Search + '%')";
                sql += whereClause;
                sqlCount += whereClause;
            }
            
            sql += $" ORDER BY {nameof(District.Name)} OFFSET {pagination.Offset} ROWS FETCH NEXT {pagination.Size} ROWS ONLY";

            var data = await _dbConnection.QueryAsync<District>(sql, new { pagination.Search });
            var total = await _dbConnection.QueryFirstOrDefaultAsync<int>(sqlCount, new { pagination.Search });

            return new PagingResult<District>(data, total);
        }
        public async Task<PagingResult<Province>> GetProvincesPagingAsync(Pagination pagination, CancellationToken cancellationToken)
        {
            var sql = $"SELECT * FROM {new Province().GetTableName()}";
            var sqlCount = sql.Replace("*", "COUNT(*)");

            if (!string.IsNullOrEmpty(pagination.Search))
            {
                var whereClause = $" WHERE {nameof(Province.Name)} LIKE '%' + @Search + '%' OR {nameof(Province.Slug)} LIKE '%' + @Search + '%'";
                sql += whereClause;
                sqlCount += whereClause;
            }

            sql += $" ORDER BY {nameof(Province.Name)} OFFSET {pagination.Offset} ROWS FETCH NEXT {pagination.Size} ROWS ONLY";

            var data = await _dbConnection.QueryAsync<Province>(sql, new { pagination.Search });
            var total = await _dbConnection.QueryFirstOrDefaultAsync<int>(sqlCount, new { pagination.Search });

            return new PagingResult<Province>(data, total);
        }

        public async Task<PagingResult<Ward>> GetWardsPagingAsync(int districtId, Pagination pagination, CancellationToken cancellationToken)
        {
            var sql = $"SELECT * FROM {new Ward().GetTableName()} WHERE 1=1";
            var sqlCount = sql.Replace("*", "COUNT(*)");

            if (districtId > 0)
            {
                sql += $" AND DistrictId = {districtId}";
                sqlCount += $" AND DistrictId = {districtId}";
            }

            if (!string.IsNullOrEmpty(pagination.Search))
            {
                var whereClause = $" AND {nameof(Ward.Name)} LIKE '%' + @Search + '%'";
                sql += whereClause;
                sqlCount += whereClause;
            }

            sql += $" ORDER BY {nameof(Ward.Name)} OFFSET {pagination.Offset} ROWS FETCH NEXT {pagination.Size} ROWS ONLY";

            var data = await _dbConnection.QueryAsync<Ward>(sql, new { pagination.Search });
            var total = await _dbConnection.QueryFirstOrDefaultAsync<int>(sqlCount, new { pagination.Search });

            return new PagingResult<Ward>(data, total);
        }
        public async Task<string> GetNameByIdAsync(int id, string type = "province", CancellationToken cancellationToken = default)
        {
            var table = "";
            switch (type)
            {
                case "province":
                    table = new Province().GetTableName();
                    break;
                case "district":
                    table = new District().GetTableName();
                    break;
                case "ward":
                    table = new Ward().GetTableName();
                    break;
            }
            if (string.IsNullOrEmpty(type))
            {
                return "";
            }

            var sql = $"SELECT {nameof(BaseLocation.Name)} FROM {table} WHERE Id = {id}";
            return await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, null);
        }

        public async Task<int> GetPidByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var sql = $"SELECT {nameof(BaseLocation.Id)} FROM {new Province().GetTableName()} WHERE {nameof(BaseLocation.Name)} like N'%{name}%'";
            return await _dbConnection.QueryFirstOrDefaultAsync<int>(sql, null);
        }

        public async Task<int> GetDidByNameAsync(string name, int pid, CancellationToken cancellationToken = default)
        {
            var sql = $"SELECT {nameof(BaseLocation.Id)} FROM {new District().GetTableName()} WHERE {nameof(BaseLocation.Name)} like N'%{name}%' AND {nameof(District.ProvinceId)} = {pid}";
            return await _dbConnection.QueryFirstOrDefaultAsync<int>(sql, null);
        }

        public async Task<int> GetWidByNameAsync(string name, int did, CancellationToken cancellationToken = default)
        {
            var sql = $"SELECT {nameof(BaseLocation.Id)} FROM {new Ward().GetTableName()} WHERE {nameof(BaseLocation.Name)} like N'%{name}%' AND {nameof(Ward.DistrictId)} = {did}";
            return await _dbConnection.QueryFirstOrDefaultAsync<int>(sql, null);
        }
    }
}
