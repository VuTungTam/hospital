using FluentValidation;
using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.HealthServices
{
    public class HealthServiceDto : BaseDto
    {
        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public string DescriptionVn { get; set; }

        public string DescriptionEn { get; set; }

        public string TypeId { get; set; }

        public string TypeNameVn { get; set; }

        public string TypeNameEn { get; set; }

        public decimal Price { get; set; }

        public int TotalStars { get; set; }

        public int TotalFeedback { get; set; }

        public float StarPoint { get; set; }

        public HealthServiceStatus Status { get; set; }

        public string StatusText => Status.GetDescription();

        public string ZoneId { get; set; }

        public string DoctorId { get; set; }

        public string FacilityId { get; set; }

        public string FacilityNameVn { get; set; }

        public string FacilityNameEn { get; set; }

        public string FacilityFullAddress { get; set; }

        public string SpecialtyId { get; set; }

        public string SpecialtyNameVn { get; set; }

        public string SpecialtyNameEn { get; set; }

        public List<ServiceTimeRuleDto> ServiceTimeRules { get; set; }

        public List<GroupServiceTimeRuleDto> GroupServiceTimeRules
        {
            get
            {
                return ServiceTimeRules
                    .GroupBy(rule => new
                    {
                        rule.StartTime,
                        rule.StartBreakTime,
                        rule.EndBreakTime,
                        rule.EndTime,
                        rule.SlotDuration,
                        rule.MaxPatients
                    })
                    .Select(group => new GroupServiceTimeRuleDto
                    {
                        StartTime = group.Key.StartTime,
                        StartBreakTime = group.Key.StartBreakTime,
                        EndBreakTime = group.Key.EndBreakTime,
                        EndTime = group.Key.EndTime,
                        SlotDuration = group.Key.SlotDuration,
                        MaxPatients = group.Key.MaxPatients,
                        DayOfWeeks = group.Select(rule => rule.DayOfWeek).Distinct().ToList()
                    })
                    .ToList();
            }
        }
    }

    public class GroupServiceTimeRuleDto
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan StartBreakTime { get; set; }

        public TimeSpan EndBreakTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int SlotDuration { get; set; }

        public int MaxPatients { get; set; }

        public List<DayOfWeek> DayOfWeeks { get; set; }
    }

    public class HealthServiceValidator : BaseAbstractValidator<HealthServiceDto>
    {
        public HealthServiceValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.NameVn).NotEmpty().WithMessage(localizer["health_service_name_vn_is_not_empty"]);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage(localizer["health_service_name_en_is_not_empty"]);
            RuleFor(x => x.DescriptionVn).NotEmpty().WithMessage(localizer["health_service_description_vn_is_not_empty"]);
            RuleFor(x => x.DescriptionEn).NotEmpty().WithMessage(localizer["health_service_description_en_is_not_empty"]);
            RuleFor(x => x.Price).NotEmpty().WithMessage(localizer["health_service_price_is_not_empty"]);
            RuleFor(x => x.Price).GreaterThan(0).WithMessage(localizer["health_service_price_must_be_greater_than_0"]);
            RuleFor(x => x.DoctorId).Must(x => long.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_doctor_id"]);
            RuleFor(x => x.TypeId).Must(x => long.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_type_id"]);
            RuleFor(x => x.SpecialtyId).Must(x => long.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_specialty_id"]);
        }
    }
}
