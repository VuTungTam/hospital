using AutoMapper;
using Hospital.Application.Dtos.Auth;
using Hospital.Application.Dtos.Bookings;
using Hospital.Application.Dtos.Customers;
using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Dtos.Employee;
using Hospital.Application.Dtos.Feedbacks;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Dtos.HealthProfiles;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Dtos.Locations;
using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.Application.Dtos.SocialNetworks;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Dtos.Symptoms;
using Hospital.Application.Models.Auth;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Entities.FacilityTypes;
using Hospital.Domain.Entities.Feedbacks;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Entities.Symptoms;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites;
using System.Reflection;
using Action = Hospital.SharedKernel.Domain.Entities.Auths.Action;
namespace Hospital.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingFromAssembly(Assembly.GetExecutingAssembly());

            //Social network
            CreateMap<SocialNetwork, SocialNetworkDto>().ReverseMap();

            //Symptom
            CreateMap<Symptom, SymptomDto>().ReverseMap();

            //ServiceType
            CreateMap<ServiceType, ServiceTypeDto>().ReverseMap();

            //HealthService
            CreateMap<HealthService, HealthServiceDto>().ReverseMap();

            //ServiceTimeRule
            CreateMap<ServiceTimeRule, ServiceTimeRuleDto>().ReverseMap();

            //Feedback
            CreateMap<Feedback, FeedbackDto>().ReverseMap();

            //HealthFacility
            CreateMap<HealthFacility, HealthFacilityDto>().ReverseMap();

            //FacilityType
            CreateMap<FacilityType, FacilityTypeDto>().ReverseMap();

            //HealthProfile
            CreateMap<HealthProfile, HealthProfileDto>().ReverseMap();

            //Specialty
            CreateMap<Specialty, SpecialtyDto>().ReverseMap();
            // Booking
            CreateMap<Booking, BookingResponseDto>()
                .ForMember(dest => dest.SymptomIds, opt => opt.MapFrom(src => src.BookingSymptoms != null
                    ? src.BookingSymptoms.Select(bs => bs.SymptomId.ToString()).ToList()
                    : new List<string>()));

            CreateMap<BookingRequestDto, Booking>()
                .ForMember(dest => dest.BookingSymptoms, opt => opt.Ignore());

            // Locations
            CreateMap<Province, ProvinceDto>().ReverseMap();
            CreateMap<District, DistrictDto>().ReverseMap();
            CreateMap<Ward, WardDto>().ReverseMap();

            // Users
            CreateMap<RegAccountRequest, Customer>();

            CreateMap<EmployeeDto, Employee>();

            CreateMap<Employee, EmployeeDto>()
                .ForMember(des => des.Roles, opt => opt.MapFrom(src => src.EmployeeRoles != null ? src.EmployeeRoles.Select(x => x.Role).ToList() : new()))
                .ForMember(des => des.Actions, opt => opt.MapFrom(src => src.EmployeeActions != null ? src.EmployeeActions.Select(x => x.Action).ToList() : new()))
                .ForMember(des => des.CanChangePassword, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Password)));

            CreateMap<DoctorDto, Doctor>();

            CreateMap<Doctor, DoctorDto>()
                .ForMember(des => des.Specialties, otp => otp.MapFrom(src => src.DoctorSpecialties != null ? src.DoctorSpecialties.Select(x => x.Specialty).ToList() : new()));

            CreateMap<Doctor, PublicDoctorDto>()
                .ForMember(des => des.Specialties, otp => otp.MapFrom(src => src.DoctorSpecialties != null ? string.Join(", ", src.DoctorSpecialties.Select(x => x.Specialty.NameVn)) : string.Empty));

            CreateMap<CustomerDto, Customer>();

            CreateMap<Customer, CustomerDto>()
                .ForMember(des => des.CanChangePassword, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Password)));

            CreateMap<Customer, CustomerNameDto>();
            // Roles
            CreateMap<Action, ActionDto>();

            CreateMap<Role, RoleDto>()
                .ForMember(des => des.Actions, opt => opt.MapFrom(src => src.RoleActions.Select(x => x.Action)));

            CreateMap<RoleDto, Role>()
                .ForMember(des => des.RoleActions, opt => opt.MapFrom(src => src.Actions.Select(x => new RoleAction { RoleId = long.Parse(src.Id), ActionId = long.Parse(x.Id) })));
        }
        private static long StringToInt64(string str) => long.TryParse(str, out var id) ? id : 0;
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
