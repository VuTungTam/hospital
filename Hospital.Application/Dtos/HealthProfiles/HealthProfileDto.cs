using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Infrastructure.Services.Sms.Utils;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.HealthProfiles
{
    public class HealthProfileDto : BaseDto
    {
        public string CICode { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public int Gender { get; set; }

        public DateTime Dob { get; set; }

        public string Pid { get; set; }

        public string Pname { get; set; }

        public string Did { get; set; }

        public string Dname { get; set; }

        public string Wid { get; set; }

        public string Wname { get; set; }

        public string Address { get; set; }

        public int Eid { get; set; }

        public string Ethinic { get; set; }

        public string OwnerId { get; set; }
    }
    public class HealthProfileDtoValidator : BaseAbstractValidator<HealthProfileDto>
    {
        public HealthProfileDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["patient_name_is_not_empty"]);
            RuleFor(x => x.Phone).NotEmpty().WithMessage(localizer["patient_phone_is_not_empty"]);
            RuleFor(x => x.Gender).NotEmpty().WithMessage(localizer["patient_gender_is_not_empty"]);
            RuleFor(x => x.Dob).Must(x => x != default && x < DateTime.Now && x > new DateTime(1950, 1, 1)).WithMessage(localizer["invalid_date_of_birth"]);
            RuleFor(x => x.Phone).Must(x => SmsUtility.IsVietnamesePhone(x)).WithMessage(localizer["invalid_phone_number"]);
            RuleFor(x => x.Pid).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_province"]);
            RuleFor(x => x.Did).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_district"]);
            RuleFor(x => x.Wid).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_ward"]);
        }
        
    }
}
