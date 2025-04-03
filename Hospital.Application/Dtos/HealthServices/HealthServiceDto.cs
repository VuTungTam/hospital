using FluentValidation;
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

        public long TypeId { get; set; }

        public decimal Price { get; set; }

        public int TotalStars { get; set; }

        public int TotalFeedback { get; set; }

        public float StarPoint { get; set; }

        public HealthServiceStatus Status { get; set; }

        public string StatusText => Status.GetDescription();

        public string ZoneId { get; set; }

        public string DoctorId { get; set; }

        public string FacilityId { get; set; }

        public string SpecialtyId { get; set; }


        public List<TimeFrame> TimeSlots ;
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
            RuleFor(x => x.ZoneId).Must(x => int.TryParse(x, out var id) && id >= 0).WithMessage(localizer["invalid_zone_id"]);
            RuleFor(x => x.FacilityId).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_facility_id"]);
            RuleFor(x => x.DoctorId).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_doctor_id"]);
            RuleFor(x => x.SpecialtyId).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_specialty_id"]);
        }
    }
}
