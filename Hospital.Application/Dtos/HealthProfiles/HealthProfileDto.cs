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
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["patient_name_is_not_empty"]);
            //RuleFor(x => x.Gender).NotEmpty().WithMessage(localizer["patient_gender_is_not_empty"]);
            RuleFor(x => x.Gender).Must(gender => gender == 0 || gender == 1).WithMessage(localizer["patient_gender_is_invalid"]);
            RuleFor(x => x.Dob).Must(x => x != default && x < DateTime.Now && x > new DateTime(1950, 1, 1)).WithMessage(localizer["invalid_date_of_birth"]);
            RuleFor(x => x.CICode).NotEmpty().WithMessage(localizer["patient_CICode_is_not_empty"]);
            RuleFor(x => x.CICode).Must((x, cicCode) => CICodeUtility.ValidateCCCD(cicCode, x.Dob.Year, x.Gender)).WithMessage(localizer["invalid_CICode"]);
        }

    }
}
