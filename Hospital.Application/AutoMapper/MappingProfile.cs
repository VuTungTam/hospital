using AutoMapper;
using Hospital.Application.Dtos.Blogs;
using Hospital.Application.Dtos.Declarations;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Dtos.Locations;
using Hospital.Application.Dtos.SocialNetworks;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Dtos.Symptoms;
using Hospital.Domain.Entities.Blogs;
using Hospital.Domain.Entities.Declarations;
using Hospital.Domain.Entities.HeathFacilities;
using Hospital.Domain.Entities.HeathServices;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Entities.Symptoms;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites;
using System.Reflection;

namespace Hospital.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingFromAssembly(Assembly.GetExecutingAssembly());
            CreateMap<Blog, BlogDto>().ReverseMap();
            CreateMap<SocialNetwork, SocialNetworkDto>().ReverseMap();
            CreateMap<Symptom, SymptomDto>().ReverseMap();
            CreateMap<ServiceType, ServiceTypeDto>().ReverseMap();
            CreateMap<HealthService, HealthServiceDto>().ReverseMap();
            CreateMap<HealthFacility, HealthFacilityDto>().ReverseMap();
            CreateMap<FacilityCategory, FacilityCategoryDto>().ReverseMap();
            CreateMap<Declaration, DeclarationDto>().ReverseMap();
            CreateMap<Specialty, SpecialtyDto>().ReverseMap();
            // Locations
            CreateMap<Province, ProvinceDto>().ReverseMap();
            CreateMap<District, DistrictDto>().ReverseMap();
            CreateMap<Ward, WardDto>().ReverseMap();
        }
        private void ApplyMappingFromAssembly(Assembly assembly)
        {
            var mapFromType = typeof(IMapFrom<>);
            var mappingMethodName = nameof(IMapFrom<object>.Mapping);
            bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;
            var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();
            var argumentTypes = new Type[] { typeof(Profile) };
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfor = type.GetMethod(mappingMethodName);
                if (methodInfor != null)
                {
                    methodInfor.Invoke(instance, new object[] { this });
                }
                else
                {
                    var interfaces = type.GetInterfaces().Where(HasInterface).ToList();
                    if (interfaces.Count > 0)
                    {
                        foreach (var @interface in interfaces)
                        {
                            var interfaceMetodInfor = @interface.GetMethod(mappingMethodName, argumentTypes);
                            interfaceMetodInfor?.Invoke(instance, new object[] { this });
                        }
                    }
                }
            }
        }
    }
}
