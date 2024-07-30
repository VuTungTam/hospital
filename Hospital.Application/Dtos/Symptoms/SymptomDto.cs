using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Symptoms
{
    public class SymptomDto : BaseDto
    {
        public string Name { get; set; }
    }
    public class SymptomDtoValidator : BaseAbstractValidator<SymptomDto>
    {
        public SymptomDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["symptom_name_is_not_empty"].Value);
        }
    }
}
