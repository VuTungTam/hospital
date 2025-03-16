using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Dtos.SocialNetworks;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Dtos.Symptoms;
using Hospital.Domain.Entities.FacilityTypes;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Entities.Symptoms;
using Hospital.SharedKernel.Application.CQRS.Queries;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Application.DI
{
    public static class QueryHandlersExtension
    {
        public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<GetAllQuery<SocialNetwork, SocialNetworkDto>, List<SocialNetworkDto>>, GetAllQueryHandler<SocialNetwork, SocialNetworkDto>>();
            services.AddScoped<IRequestHandler<GetByIdQuery<SocialNetwork, SocialNetworkDto>, SocialNetworkDto>, GetByIdQueryHandler<SocialNetwork, SocialNetworkDto>>();

            services.AddScoped<IRequestHandler<GetAllQuery<Symptom, SymptomDto>, List<SymptomDto>>, GetAllQueryHandler<Symptom, SymptomDto>>();
            services.AddScoped<IRequestHandler<GetByIdQuery<Symptom, SymptomDto>, SymptomDto>, GetByIdQueryHandler<Symptom, SymptomDto>>();
            services.AddScoped<IRequestHandler<GetPagingQuery<Symptom, SymptomDto>,PaginationResult<SymptomDto>>, GetPagingQueryHandler<Symptom, SymptomDto>>();

            services.AddScoped<IRequestHandler<GetAllQuery<ServiceType, ServiceTypeDto>, List<ServiceTypeDto>>, GetAllQueryHandler<ServiceType, ServiceTypeDto>>();
            services.AddScoped<IRequestHandler<GetByIdQuery<ServiceType, ServiceTypeDto>, ServiceTypeDto>, GetByIdQueryHandler<ServiceType, ServiceTypeDto>>();
            services.AddScoped<IRequestHandler<GetPagingQuery<ServiceType, ServiceTypeDto>, PaginationResult<ServiceTypeDto>>, GetPagingQueryHandler<ServiceType, ServiceTypeDto>>();

            services.AddScoped<IRequestHandler<GetAllQuery<HealthService, HealthServiceDto>, List<HealthServiceDto>>, GetAllQueryHandler<HealthService, HealthServiceDto>>();
            services.AddScoped<IRequestHandler<GetByIdQuery<HealthService, HealthServiceDto>, HealthServiceDto>, GetByIdQueryHandler<HealthService, HealthServiceDto>>();
            services.AddScoped<IRequestHandler<GetPagingQuery<HealthService, HealthServiceDto>, PaginationResult<HealthServiceDto>>, GetPagingQueryHandler<HealthService, HealthServiceDto>>();

            services.AddScoped<IRequestHandler<GetAllQuery<FacilityType, FacilityCategoryDto>, List<FacilityCategoryDto>>, GetAllQueryHandler<FacilityType, FacilityCategoryDto>>();
            services.AddScoped<IRequestHandler<GetByIdQuery<FacilityType, FacilityCategoryDto>, FacilityCategoryDto>, GetByIdQueryHandler<FacilityType, FacilityCategoryDto>>();
            services.AddScoped<IRequestHandler<GetPagingQuery<FacilityType, FacilityCategoryDto>, PaginationResult<FacilityCategoryDto>>, GetPagingQueryHandler<FacilityType, FacilityCategoryDto>>();

            services.AddScoped<IRequestHandler<GetAllQuery<HealthFacility, HealthFacilityDto>, List<HealthFacilityDto>>, GetAllQueryHandler<HealthFacility, HealthFacilityDto>>();
            services.AddScoped<IRequestHandler<GetByIdQuery<HealthFacility, HealthFacilityDto>, HealthFacilityDto>, GetByIdQueryHandler<HealthFacility, HealthFacilityDto>>();
            services.AddScoped<IRequestHandler<GetPagingQuery<HealthFacility, HealthFacilityDto>, PaginationResult<HealthFacilityDto>>, GetPagingQueryHandler<HealthFacility, HealthFacilityDto>>();

            services.AddScoped<IRequestHandler<GetAllQuery<Specialty, SpecialtyDto>, List<SpecialtyDto>>, GetAllQueryHandler<Specialty, SpecialtyDto>>();
            services.AddScoped<IRequestHandler<GetByIdQuery<Specialty, SpecialtyDto>, SpecialtyDto>, GetByIdQueryHandler< Specialty, SpecialtyDto>>();
            services.AddScoped<IRequestHandler<GetPagingQuery<Specialty, SpecialtyDto>, PaginationResult<SpecialtyDto>>, GetPagingQueryHandler<Specialty, SpecialtyDto>>();

            services.AddScoped<IRequestHandler<GetAllQuery<Specialty, SpecialtyDto>, List<SpecialtyDto>>, GetAllQueryHandler<Specialty, SpecialtyDto>>();
            services.AddScoped<IRequestHandler<GetByIdQuery<Specialty, SpecialtyDto>, SpecialtyDto>, GetByIdQueryHandler<Specialty, SpecialtyDto>>();
            services.AddScoped<IRequestHandler<GetPagingQuery<Specialty, SpecialtyDto>, PaginationResult<SpecialtyDto>>, GetPagingQueryHandler<Specialty, SpecialtyDto>>();
            return services;
        }
    }
}
