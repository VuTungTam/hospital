using System.Reflection;
using AutoMapper;
using Hospital.Application.Dtos.Articles;
using Hospital.Application.Dtos.Auth;
using Hospital.Application.Dtos.Bookings;
using Hospital.Application.Dtos.CancelReasons;
using Hospital.Application.Dtos.Customers;
using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Dtos.Employee;
using Hospital.Application.Dtos.Feedbacks;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Dtos.HealthProfiles;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Dtos.Images;
using Hospital.Application.Dtos.Locations;
using Hospital.Application.Dtos.Metas;
using Hospital.Application.Dtos.Notifications;
using Hospital.Application.Dtos.Payments;
using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Dtos.SystemConfigurations;
using Hospital.Application.Dtos.TimeSlots;
using Hospital.Application.Dtos.Zones;
using Hospital.Application.Models.Auth;
using Hospital.Domain.Entities.Articles;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.CancelReasons;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Entities.FacilityTypes;
using Hospital.Domain.Entities.Feedbacks;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.Images;
using Hospital.Domain.Entities.Metas;
using Hospital.Domain.Entities.Payments;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Entities.TimeSlots;
using Hospital.Domain.Entities.Zones;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Entities.Systems;
using Hospital.SharedKernel.Domain.Models.Auths;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Action = Hospital.SharedKernel.Domain.Entities.Auths.Action;
namespace Hospital.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingFromAssembly(Assembly.GetExecutingAssembly());

            // App configs
            CreateMap<SystemConfigurationDto, SystemConfiguration>()
                .ForMember(des => des.BookingNotificationBccEmails, opt => opt.MapFrom(src => string.Join(",", src.BookingNotificationBccEmails ?? new())));

            CreateMap<SystemConfiguration, SystemConfigurationDto>()
                .ForMember(des => des.BookingNotificationBccEmails, opt => opt.MapFrom(src => SplitToList(src.BookingNotificationBccEmails, ",")));

            //LoginHistory
            CreateMap<LoginHistory, LoginHistoryDto>();

            // Notifications
            CreateMap<Notification, NotificationDto>().ReverseMap();

            // Roles
            CreateMap<Action, ActionDto>();

            CreateMap<ActionWithExcludeValue, ActionDto>();

            CreateMap<Role, RoleDto>()
                .ForMember(des => des.Actions, opt => opt.MapFrom(src => src.RoleActions.Select(x => x.Action)));

            CreateMap<RoleDto, Role>()
                .ForMember(des => des.RoleActions, opt => opt.MapFrom(src => src.Actions.Select(x => new RoleAction { RoleId = long.Parse(src.Id), ActionId = long.Parse(x.Id) })));

            // Locations
            CreateMap<Province, ProvinceDto>().ReverseMap();
            CreateMap<District, DistrictDto>().ReverseMap();
            CreateMap<Ward, WardDto>().ReverseMap();

            // Users
            CreateMap<RegAccountRequest, Customer>();

            CreateMap<EmployeeDto, Employee>();

            CreateMap<AdminDto, Employee>();

            CreateMap<Employee, EmployeeDto>()
                .ForMember(des => des.Roles, opt => opt.MapFrom(src => src.EmployeeRoles != null ? src.EmployeeRoles.Select(x => x.Role).ToList() : new()))
                .ForMember(des => des.CanChangePassword, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Password)));

            CreateMap<DoctorDto, Doctor>()
                .ForMember(dest => dest.DoctorSpecialties, opt => opt.Ignore());
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.SpecialtyIds, opt => opt.MapFrom(src => src.DoctorSpecialties != null
                        ? src.DoctorSpecialties.Select(bs => bs.SpecialtyId.ToString()).ToList()
                        : new List<string>()));

            CreateMap<Doctor, PublicDoctorDto>()
                .ForMember(des => des.SpecialtyVns, otp => otp.MapFrom(src => src.DoctorSpecialties != null ? string.Join(", ", src.DoctorSpecialties.Select(x => x.Specialty.NameVn)) : string.Empty))
                .ForMember(des => des.SpecialtyEns, otp => otp.MapFrom(src => src.DoctorSpecialties != null ? string.Join(", ", src.DoctorSpecialties.Select(x => x.Specialty.NameEn)) : string.Empty));

            CreateMap<CustomerDto, Customer>();

            CreateMap<Customer, CustomerDto>()
                .ForMember(des => des.CanChangePassword, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Password)));

            CreateMap<Customer, CustomerNameDto>();

            //ServiceType
            CreateMap<ServiceType, ServiceTypeDto>().ReverseMap();

            //HealthService
            CreateMap<HealthService, HealthServiceDto>();

            CreateMap<HealthServiceDto, HealthService>();

            //ServiceTimeRule
            CreateMap<ServiceTimeRule, ServiceTimeRuleDto>().ReverseMap();

            CreateMap<TimeSlot, TimeSlotDto>().ReverseMap();
            CreateMap<TimeSlot, TimeSlotBookedDto>().ReverseMap();

            //Feedback
            CreateMap<Feedback, FeedbackDto>().ReverseMap();

            //HealthFacility
            CreateMap<HealthFacility, HealthFacilityDto>()
                .ForMember(dest => dest.SpecialtyIds, opt => opt.MapFrom(src => src.FacilitySpecialties != null
                        ? src.FacilitySpecialties.Select(bs => bs.SpecialtyId.ToString()).ToList()
                        : new List<string>()))
                .ForMember(dest => dest.TypeIds, opt => opt.MapFrom(src => src.FacilityTypeMappings != null
                        ? src.FacilityTypeMappings.Select(bs => bs.TypeId.ToString()).ToList()
                        : new List<string>()))
                 .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src =>
                        string.Join(", ", new[] {
                            src.Address,
                            src.Wname,
                            src.Dname,
                            src.Pname
                        }.Where(s => !string.IsNullOrWhiteSpace(s)))));

            CreateMap<HealthFacilityDto, HealthFacility>();

            CreateMap<HealthFacility, FacilityNameDto>();

            //FacilityType
            CreateMap<FacilityType, FacilityTypeDto>().ReverseMap();

            //HealthProfile
            CreateMap<HealthProfile, HealthProfileDto>().ReverseMap();
            CreateMap<HealthProfileDto, HealthProfile>()
                .ForMember(des => des.OwnerId, opt => opt.MapFrom(src => StringToInt64(src.OwnerId)));
            //Specialty
            CreateMap<Specialty, SpecialtyDto>().ReverseMap();

            // Booking
            CreateMap<Booking, BookingDto>();

            CreateMap<BookingDto, Booking>();

            //Zone
            CreateMap<Zone, ZoneDto>()
                .ForMember(dest => dest.SpecialtyIds, opt => opt.MapFrom(src => src.ZoneSpecialties != null
                        ? src.ZoneSpecialties.Select(bs => bs.SpecialtyId.ToString()).ToList()
                        : new List<string>()));

            CreateMap<ZoneDto, Zone>()
                .ForMember(dest => dest.ZoneSpecialties, opt => opt.Ignore());
            // Scripts
            CreateMap<Script, ScriptDto>().ReverseMap();

            // Metas
            CreateMap<Meta, MetaDto>().ReverseMap();

            //Images
            CreateMap<Image, ImageDto>().ReverseMap();

            //CancelReason
            CreateMap<CancelReason, CancelReasonDto>().ReverseMap();

            //CancelReason
            CreateMap<Payment, PaymentDto>().ReverseMap();

            //Article
            CreateMap<ArticleDto, Article>().ReverseMap();

        }
        private static long StringToInt64(string str) => long.TryParse(str, out var id) ? id : 0;

        private static List<string> SplitToList(string input, string separator)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new();
            }

            return input.Split(separator).ToList();
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
