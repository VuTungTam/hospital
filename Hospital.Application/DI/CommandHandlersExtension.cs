using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Hospital.Application.Dtos.Blogs;
using Hospital.Domain.Entities.Blogs;
using Hospital.SharedKernel.Application.CQRS.Queries;
using Hospital.Application.Dtos.SocialNetworks;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.SharedKernel.Application.CQRS.Commands;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.Domain.Entities.Symptoms;
using Hospital.Application.Dtos.Symptoms;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Domain.Entities.Specialties;
using Hospital.Application.Dtos.Specialties;

namespace Hospital.Application.DI
{
    public static class CommandHandlersExtension
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {

            services.AddScoped<IRequestHandler<AddCommand<SocialNetwork, SocialNetworkDto>, string>, AddCommandHandler<SocialNetwork, SocialNetworkDto, string>>();
            services.AddScoped<IRequestHandler<UpdateCommand<SocialNetwork, SocialNetworkDto>, Unit>, UpdateCommandHandler<SocialNetwork, SocialNetworkDto, Unit>>();
            services.AddScoped<IRequestHandler<DeleteCommand<SocialNetwork>, Unit>, DeleteCommandHandler<SocialNetwork, Unit>>();

            services.AddScoped<IRequestHandler<AddCommand<Symptom, SymptomDto>, string>, AddCommandHandler<Symptom, SymptomDto, string>>();
            services.AddScoped<IRequestHandler<UpdateCommand<Symptom, SymptomDto>, Unit>, UpdateCommandHandler<Symptom, SymptomDto, Unit>>();
            services.AddScoped<IRequestHandler<DeleteCommand<Symptom>, Unit>, DeleteCommandHandler<Symptom, Unit>>();

            services.AddScoped<IRequestHandler<AddCommand<ServiceType, ServiceTypeDto>, string>, AddCommandHandler<ServiceType, ServiceTypeDto, string>>();
            services.AddScoped<IRequestHandler<UpdateCommand<ServiceType, ServiceTypeDto>, Unit>, UpdateCommandHandler<ServiceType, ServiceTypeDto, Unit>>();
            services.AddScoped<IRequestHandler<DeleteCommand<ServiceType>, Unit>, DeleteCommandHandler<ServiceType, Unit>>();

            services.AddScoped<IRequestHandler<AddCommand<HealthService, HealthServiceDto>, string>, AddCommandHandler<HealthService, HealthServiceDto, string>>();
            services.AddScoped<IRequestHandler<UpdateCommand<HealthService, HealthServiceDto>, Unit>, UpdateCommandHandler<HealthService, HealthServiceDto, Unit>>();
            services.AddScoped<IRequestHandler<DeleteCommand<HealthService>, Unit>, DeleteCommandHandler<HealthService, Unit>>();

            services.AddScoped<IRequestHandler<AddCommand<FacilityCategory, FacilityCategoryDto>, string>, AddCommandHandler<FacilityCategory, FacilityCategoryDto, string>>();
            services.AddScoped<IRequestHandler<UpdateCommand<FacilityCategory, FacilityCategoryDto>, Unit>, UpdateCommandHandler<FacilityCategory, FacilityCategoryDto, Unit>>();
            services.AddScoped<IRequestHandler<DeleteCommand<FacilityCategory>, Unit>, DeleteCommandHandler<FacilityCategory, Unit>>();

            services.AddScoped<IRequestHandler<AddCommand<HealthFacility, HealthFacilityDto>, string>, AddCommandHandler<HealthFacility, HealthFacilityDto, string>>();
            services.AddScoped<IRequestHandler<UpdateCommand<HealthFacility, HealthFacilityDto>, Unit>, UpdateCommandHandler<HealthFacility, HealthFacilityDto, Unit>>();
            services.AddScoped<IRequestHandler<DeleteCommand<HealthFacility>, Unit>, DeleteCommandHandler<HealthFacility, Unit>>();

            services.AddScoped<IRequestHandler<AddCommand<Specialty, SpecialtyDto>, string>, AddCommandHandler<Specialty, SpecialtyDto, string>>();
            services.AddScoped<IRequestHandler<UpdateCommand<Specialty, SpecialtyDto>, Unit>, UpdateCommandHandler<Specialty, SpecialtyDto, Unit>>();
            services.AddScoped<IRequestHandler<DeleteCommand<Specialty>, Unit>, DeleteCommandHandler<Specialty, Unit>>();
            return services;
        }
    }
}
