using FluentValidation;
using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Extensions;

namespace Hospital.Application.Dtos.HealthServices
{
    public class HealthServiceDto : BaseDto
    {
        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public string TypeId { get; set; }

        public string TypeNameVn { get; set; }

        public string TypeNameEn { get; set; }

        public List<string> Days { get; set; }

        public decimal Price { get; set; }

        public int TotalStars { get; set; }

        public int TotalFeedback { get; set; }

        public float StarPoint { get; set; }

        public HealthServiceStatus Status { get; set; }

        public string StatusText => Status.GetDescription();

        public string StatusTextEn => Status.GetDisplayName();

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
    }

    public class HealthServiceValidator : BaseAbstractValidator<HealthServiceDto>
    {
        public HealthServiceValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.NameVn).NotEmpty().WithMessage(localizer["health_service_name_vn_is_not_empty"]);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage(localizer["health_service_name_en_is_not_empty"]);
            RuleFor(x => x.Price).NotEmpty().WithMessage(localizer["health_service_price_is_not_empty"]);
            RuleFor(x => x.Price).GreaterThan(0).WithMessage(localizer["health_service_price_must_be_greater_than_0"]);
            RuleFor(x => x.DoctorId).Must(x => long.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_doctor_id"]);
            RuleFor(x => x.TypeId).Must(x => long.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_type_id"]);
            RuleFor(x => x.SpecialtyId).Must(x => long.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_specialty_id"]);
        }
    }
}
