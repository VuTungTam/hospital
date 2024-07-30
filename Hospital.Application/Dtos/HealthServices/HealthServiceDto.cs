using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.HealthServices
{
    public class HealthServiceDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long TypeId { get; set; }
        public decimal Price { get; set; }
        public long FacilitySpecialtyId { get; set; }
    }
    public class HeathServiceValidator : BaseAbstractValidator<HealthServiceDto>
    {
        public HeathServiceValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["health_service_name_is_not_empty"]);
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizer["health_service_description_is_not_empty"]);
            RuleFor(x => x.Price).NotEmpty().WithMessage(localizer["health_service_price_is_not_empty"]);
            RuleFor(x => x.Price).GreaterThan(0).WithMessage(localizer["health_service_price_must_be_greater_than_0"]);
        }
    }
}
