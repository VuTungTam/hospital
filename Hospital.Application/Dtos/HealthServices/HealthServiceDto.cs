using FluentValidation;
using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Extensions;

namespace Hospital.Application.Dtos.HealthServices
{
    public class HealthServiceDto : BaseDto
    {
        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public string TypeId { get; set; }

        public string TypeNameVn { get; set; }

        public string TypeNameEn { get; set; }

        public List<string> Days { get; set; }

        public decimal Price { get; set; }

        public int TotalStars { get; set; }

        public int TotalFeedback { get; set; }

        public float StarPoint { get; set; }

        public HealthServiceStatus Status { get; set; }

        public string StatusText => Status.GetDescription();

        public string StatusTextEn => Status.GetDisplayName();

        public string ZoneId { get; set; }

        public string DoctorId { get; set; }

        public string FacilityId { get; set; }

        public string FacilityNameVn { get; set; }

        public string FacilityNameEn { get; set; }

        public string FacilityFullAddress { get; set; }

        public string SpecialtyId { get; set; }

        public string SpecialtyNameVn { get; set; }

        public string SpecialtyNameEn { get; set; }

        public List<ServiceTimeRuleDto> ServiceTimeRules { get; set; }
    }

    public class HealthServiceValidator : BaseAbstractValidator<HealthServiceDto>
    {
        public HealthServiceValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.NameVn)
    .NotEmpty().WithMessage(localizer["HealthService.NameVnIsNotEmpty"]);

            RuleFor(x => x.NameEn)
                .NotEmpty().WithMessage(localizer["HealthService.NameEnIsNotEmpty"]);

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage(localizer["HealthService.PriceIsNotEmpty"]);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage(localizer["HealthService.PriceMustBeGreaterThanZero"]);

            RuleFor(x => x.DoctorId)
                .Must(x => long.TryParse(x, out var id) && id > 0)
                .WithMessage(localizer["HealthService.InvalidDoctor"]);

            RuleFor(x => x.TypeId)
                .Must(x => long.TryParse(x, out var id) && id > 0)
                .WithMessage(localizer["HealthService.InvalidType"]);

            RuleFor(x => x.SpecialtyId)
                .Must(x => long.TryParse(x, out var id) && id > 0)
                .WithMessage(localizer["HealthService.InvalidSpecialty"]);
        }
    }
}
