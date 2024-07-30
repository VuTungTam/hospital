using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Specialties
{
    public class SpecialtyDto : BaseDto
    {
        public string Name { get; set; }
    }
    public class SpecialtyValidator : BaseAbstractValidator<SpecialtyDto>
    {
        public SpecialtyValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["specialty_name_is_not_empty"]);
        }
    }
}
