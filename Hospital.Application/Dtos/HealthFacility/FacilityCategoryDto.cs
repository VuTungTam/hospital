using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.HealthFacility
{
    public class FacilityCategoryDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class FacilityCategoryValidator : BaseAbstractValidator<FacilityCategoryDto>
    {
        public FacilityCategoryValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["facility_category_name_is_not_empty"]);
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizer["facility_category_description_is_not_empty"]);
        }
    }
}
