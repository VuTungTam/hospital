using FluentValidation;
using Hospital.Application.Dtos.Specialties;
using Hospital.Resource.Properties;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Zones
{
    public class ZoneDto : BaseDto
    {
        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public string LocationVn { get; set; }

        public string LocationEn { get; set; }

        public List<SpecialtyDto> Specialties { get; set; }

        public List<string> SpecialtyIds { get; set; }

        public string ListSpecialtyNameVns => Specialties != null && Specialties.Any()
           ? string.Join(", ", Specialties
               .Where(s => s != null && s.NameVn != null)
               .Select(s => s.NameVn))
           : "Chưa có thông tin";


        public string ListSpecialtyNameEns => Specialties != null && Specialties.Any()
            ? string.Join(", ", Specialties.Select(s => s.NameEn))
            : "No data";


        public long FacilityId { get; set; }
    }
    public class ZoneValidator : AbstractValidator<ZoneDto>
    {
        public ZoneValidator(IStringLocalizer<Resources> localizer)
        {
            RuleFor(x => x.NameVn)
                .NotEmpty()
                .WithMessage(localizer["Zone.NameVnIsNotEmpty"]);

            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage(localizer["Zone.NameEnIsNotEmpty"]);

            RuleFor(x => x.LocationVn)
                .NotEmpty()
                .WithMessage(localizer["Zone.LocationVnIsNotEmpty"]);

            RuleFor(x => x.LocationEn)
                .NotEmpty()
                .WithMessage(localizer["Zone.LocationEnIsNotEmpty"]);

            RuleFor(x => x.SpecialtyIds)
                .NotNull()
                .WithMessage(localizer["Zone.SpecialtiesIsRequired"])
                .Must(x => x.Any())
                .WithMessage(localizer["Zone.SpecialtiesIsRequired"]);
        }
    }
}
