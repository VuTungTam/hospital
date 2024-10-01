using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Symptoms
{
    public class SymptomDto : BaseDto
    {
        public string NameVn { get; set; }
        public string NameEn { get; set; }
    }
    public class SymptomDtoValidator : BaseAbstractValidator<SymptomDto>
    {
        public SymptomDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.NameVn).NotEmpty().WithMessage(localizer["symptom_name_vn_is_not_empty"].Value);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage(localizer["symptom_name_en_is_not_empty"].Value);
        }
    }
}
