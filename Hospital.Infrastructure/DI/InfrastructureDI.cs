using Hospital.Application.Repositories.Interfaces.Articles;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Application.Repositories.Interfaces.Auth.Roles;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.Distances;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Sequences;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.SocialNetworks;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Infra.EFConfigurations;
using Hospital.Infra.Repositories;
using Hospital.Infrastructure.Events.Dispatchers;
using Hospital.Infrastructure.Repositories.AppConfigs;
using Hospital.Infrastructure.Repositories.Articles;
using Hospital.Infrastructure.Repositories.Auth;
using Hospital.Infrastructure.Repositories.Bookings;
using Hospital.Infrastructure.Repositories.Customers;
using Hospital.Infrastructure.Repositories.Distances;
using Hospital.Infrastructure.Repositories.Employees;
using Hospital.Infrastructure.Repositories.HealthFacilities;
using Hospital.Infrastructure.Repositories.HealthProfiles;
using Hospital.Infrastructure.Repositories.HealthServices;
using Hospital.Infrastructure.Repositories.Locations;
using Hospital.Infrastructure.Repositories.ServiceTimeRules;
using Hospital.Infrastructure.Repositories.SocialNetworks;
using Hospital.Infrastructure.Repositories.Specialities;
using Hospital.Infrastructure.Repositories.Specilities;
using Hospital.Infrastructure.Repositories.Symptoms;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Application.Repositories.Interface.AppConfigs;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Extensions;
using Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Maps;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Infrastructure.DI
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructureService
            (this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(
                options =>
                    options.UseSqlServer(configuration.GetConnectionString("RootDatabase") ??
                    throw new InvalidOperationException("Not found connection string"),
                    b => b.MigrationsAssembly("Hospital.Api"))
                );
            // Events
            services.AddScoped<IEventDispatcher, EventDispatcher>();

            // System configurations
            services.AddScoped<ISystemConfigurationReadRepository, SystemConfigurationReadRepository>();
            //services.AddScoped<ISystemConfigurationWriteRepository, SystemConfigurationWriteRepository>();

            // Dapper
            services.AddDbConenctionService(configuration);

            // Google Maps
            services.AddScoped<IGoogleMapsService, GoogleMapsService>();

            // Base
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

            // Specialty
            services.AddScoped<ISpecialtyReadRepository, SpecialityReadRepository>();
            services.AddScoped<ISpecialtyWriteRepository, SpecialityWriteRepository>();

            // HealthFacility
            services.AddScoped<IHealthFacilityReadRepository, HealthFacilityReadRepository>();
            services.AddScoped<IHealthFacilityWriteRepository, HealthFacilityWriteRepository>();
            services.AddScoped<IFacilityCategoryReadRepository, FacilityCategotyReadRepository>();

            // HealthFacility
            services.AddScoped<IArticleReadRepository, ArticleReadRepository>();
            services.AddScoped<IArticleWriteRepository, ArticleWriteRepository>();

            //Social Network
            services.AddScoped<ISocialNetworkReadRepository, SocialNetworkReadRepository>();
            services.AddScoped<ISocialNetworkWriteRepository, SocialNetworkWriteRepository>();

            //Symptom 
            services.AddScoped<ISymptomReadRepository, SymptomReadRepository>();
            services.AddScoped<ISymptomWriteRepository, SymptomWriteRepository>();

            //Location
            services.AddScoped<ILocationReadRepository, LocationReadRepository>();

            //Health Service
            services.AddScoped<IHealthServiceReadRepository, HealthServiceReadRepository>();
            services.AddScoped<IHealthServiceWriteRepository, HealthServiceWriteRepository>();
            services.AddScoped<IServiceTypeReadRepository, ServiceTypeReadRepository>();

            //Service Time Rule
            services.AddScoped<IServiceTimeRuleReadRepository, ServiceTimeRuleReadRepository>();
            services.AddScoped<IServiceTimeRuleWriteRepository, ServiceTimeRuleWriteRepository>();

            //HealthProfile
            services.AddScoped<IHealthProfileReadRepository, HealthProfileReadRepository>();
            services.AddScoped<IHealthProfileWriteRepository, HealthProfileWriteRepository>();

            //Booking
            services.AddScoped<IBookingReadRepository, BookingReadRepository>();
            services.AddScoped<IBookingWriteRepository, BookingWriteRepository>();

            // Sequences
            services.AddScoped<ISequenceRepository, SequenceRepository>();

            // Auth
            services.AddScoped<IAuthRepository, AuthRepository>();

            // Roles
            services.AddScoped<IRoleReadRepository, RoleReadRepository>();
            services.AddScoped<IRoleWriteRepository, RoleWriteRepository>();
            services.AddScoped<IActionReadRepository, ActionReadRepository>();

            // Users


            // Employees
            services.AddScoped<IEmployeeReadRepository, EmployeeReadRepository>();
            services.AddScoped<IEmployeeWriteRepository, EmployeeWriteRepository>();

            // Customers
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();

            // Distances
            services.AddScoped<IDistanceRepository, DistanceRepository>();

            return services;
        }
    }
}
