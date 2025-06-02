using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Infrastructure.Services.CIID;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.HealthProfiles
{
    public class HealthProfileDto : BaseDto
    {
        public string Code { get; set; }

        public string CICode { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int Gender { get; set; }

        public DateTime Dob { get; set; }

        public string Pid { get; set; }

        public string Pname { get; set; }

        public string Did { get; set; }

        public string Dname { get; set; }

        public string Wid { get; set; }

        public string Wname { get; set; }

        public string Address { get; set; }

        public string FullAddress => Address + ", " + Wname + ", " + Dname + ", " + Pname;

        public string OwnerId { get; set; }
    }
    public class HealthProfileDtoValidator : BaseAbstractValidator<HealthProfileDto>
    {
        public HealthProfileDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["HealthProfile.NameIsNotEmpty"]);

            RuleFor(x => x.Gender)
                .Must(gender => gender == 0 || gender == 1)
                .WithMessage(localizer["HealthProfile.GenderIsInvalid"]);

            RuleFor(x => x.Dob)
                .Must(x => x != default && x < DateTime.Now && x > new DateTime(1950, 1, 1))
                .WithMessage(localizer["HealthProfile.InvalidDateOfBirth"]);

            RuleFor(x => x.CICode)
                .Must((x, cicCode) =>
                    string.IsNullOrWhiteSpace(cicCode) || CICodeUtility.ValidateCCCD(cicCode, x.Dob.Year, x.Gender))
                .WithMessage(localizer["HealthProfile.InvalidCICode"]);

        }
    }
}
