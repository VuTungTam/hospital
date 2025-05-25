using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Domain.Specifications.Actions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Models.Auths;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Dapper;
using Hospital.SharedKernel.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Action = Hospital.SharedKernel.Domain.Entities.Auths.Action;
namespace Hospital.Infrastructure.Repositories.Auth
{
  public class ActionReadRepository : ReadRepository<Action>, IActionReadRepository
  {
    public ActionReadRepository(IServiceProvider serviceProvider, IStringLocalizer<Resources> localizer, IRedisCache redisCache) : base(serviceProvider, localizer, redisCache)
    {
    }

    public async Task<Action> GetMasterAsync(CancellationToken cancellationToken)
    {
      var cacheEntry = CacheManager.MasterAction;
      var spec = new ActionByCodeEqualsSpecification("master");
      var valueFactory = () => _dbSet.FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);

      return await _redisCache.GetOrSetAsync(cacheEntry.Key, valueFactory, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken);
    }

    public async Task<List<ActionWithExcludeValue>> GetActionsByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken = default)
    {
      var dapper = _serviceProvider.GetRequiredService<IDbConnection>();

      var employeeTable = new Employee().GetTableName();
      var employeeRoleTable = new EmployeeRole().GetTableName();
      var roleActionTable = new RoleAction().GetTableName();
      var actionTable = new Action().GetTableName();

      var sql = @$"
    SELECT a.*, 0 as IsExclude FROM (
      SELECT t2.*, ra.{nameof(RoleAction.ActionId)} FROM (
        SELECT t.*, ur.{nameof(EmployeeRole.RoleId)} FROM (
          SELECT u.Id as EmployeeId FROM {employeeTable} u 
          WHERE Id = {employeeId} AND u.IsDeleted = 0
        ) t
        INNER JOIN {employeeRoleTable} ur 
        ON t.EmployeeId = ur.{nameof(EmployeeRole.EmployeeId)} 
        AND ur.IsDeleted = 0
      ) t2
      INNER JOIN {roleActionTable} ra 
      ON t2.RoleId = ra.RoleId
    ) t3
    INNER JOIN {actionTable} a 
    ON t3.ActionId = a.Id
";



      var actions = await dapper.QueryAsync<ActionWithExcludeValue>(sql);
      var excludes = actions.Where(x => x.IsExclude).ToList();
      var excludeIds = excludes.Select(x => x.Id).ToList();

      return actions.Where(x => !excludeIds.Contains(x.Id)).ToList();
    }
  }
}
