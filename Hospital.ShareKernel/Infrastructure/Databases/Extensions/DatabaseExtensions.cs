using Hospital.SharedKernel.Infrastructure.Databases.Dapper;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.SharedKernel.Infrastructure.Databases.Extensions
{
    public static class DatabaseExtensions
    {
        public static void SetConnectionStrings(IConfiguration configuration)
        {
            ConnectionStringConfig.ConnectionStrings = configuration.GetRequiredSection("ConnectionStrings").Get<Dictionary<string, string>>();
        }

        public static IServiceCollection AddDbConnectionService(this IServiceCollection services, IConfiguration configuration)
        {
            SetConnectionStrings(configuration);
            return services.AddScoped<IDbConnection, DbConnection>();
        }
    }
}
