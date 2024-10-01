using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
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
        public long FacilitySpecialtyId { get; set; }
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
        }
    }
}
