using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.HealthFacility
{
    public class FacilityCategoryDto : BaseDto
    {
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string DescriptionVn { get; set; }
        public string DescriptionEn { get; set; }
    }
    public class FacilityCategoryValidator : BaseAbstractValidator<FacilityCategoryDto>
    {
        public FacilityCategoryValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.NameVn).NotEmpty().WithMessage(localizer["facility_category_name_vn_is_not_empty"]);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage(localizer["facility_category_name_en_is_not_empty"]);
            RuleFor(x => x.DescriptionEn).NotEmpty().WithMessage(localizer["facility_category_description_en_is_not_empty"]);
            RuleFor(x => x.DescriptionVn).NotEmpty().WithMessage(localizer["facility_category_description_vn_is_not_empty"]);
        }
    }
}
