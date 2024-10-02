using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Specialties
{
    public class SpecialtyDto : BaseDto
    {
        public string NameVn { get; set; }
        public string NameEn { get; set; }
    }
    public class SpecialtyValidator : BaseAbstractValidator<SpecialtyDto>
    {
        public SpecialtyValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.NameVn).NotEmpty().WithMessage(localizer["specialty_name_vn_is_not_empty"]);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage(localizer["specialty_name_en_is_not_empty"]);
        }
    }
}
