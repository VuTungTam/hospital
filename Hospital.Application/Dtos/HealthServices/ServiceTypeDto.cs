using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.HealthServices
{
    public class ServiceTypeDto : BaseDto
    {
        public string Name { get; set; }
    }
    public class ServiceTypeValidator : BaseAbstractValidator<ServiceTypeDto>
    {
        public ServiceTypeValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["service_type_name_is_not_empty"]);
        }
    }
}
