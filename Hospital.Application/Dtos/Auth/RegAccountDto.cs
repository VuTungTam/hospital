using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Infrastructure.Services.Sms.Utils;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Auth
{
    public class RegAccountDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string RepeatPassword { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public DateTime Dob { get; set; }

        public string Pid { get; set; }

        public string Did { get; set; }

        public string Wid { get; set; }
    }
    public class RegAccountDtoValidator : BaseAbstractValidator<RegAccountDto>
    {
        public RegAccountDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Email)
                .Must(e => EmailUtility.IsEmail(e))
                .WithMessage(localizer["common_email_is_not_valid"]);
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(localizer["auth_password_must_not_be_empty"]);

            RuleFor(x => x.RepeatPassword)
                .NotEmpty()
                .WithMessage(localizer["auth_repeat_password_must_not_be_empty"]);

            RuleFor(x => x)
                .Must(x => x.Password.Equals(x.RepeatPassword))
                .WithMessage(localizer["auth_pwd_n_cpwd_must_same"]);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizer["auth_full_name_must_not_be_empty"]);
            RuleFor(x => x)
               .Must(x =>
               {
                   if (string.IsNullOrEmpty(x.Phone))
                   {
                       return true;
                   }
                   return SmsUtility.IsVietnamesePhone(x.Phone);
               })
               .WithMessage(localizer["auth_phone_is_not_valid"]);
        }
    }
}
