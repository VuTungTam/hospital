using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Infrastructure.Services.Sms.Utils;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Models.Auth
{
    public class RegAccountRequest
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

    public class RegAccountDtoValidator : BaseAbstractValidator<RegAccountRequest>
    {
        public RegAccountDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Email)
                .Must(e => EmailUtility.IsEmail(e))
                .WithMessage(localizer["CommonMessage.EmailIsNotValid"]);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(localizer["Authentication.PasswordMustNotBeEmpty"]);

            RuleFor(x => x.RepeatPassword)
                .NotEmpty()
                .WithMessage(localizer["Authentication.RepeatPasswordMustNotBeEmpty"]);

            RuleFor(x => x)
                .Must(x => x.Password.Equals(x.RepeatPassword))
                .WithMessage(localizer["Authentication.PasswordAndRepeatPasswordMustSame"]);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizer["Authentication.FullNameMustNotBeEmpty"]);

            RuleFor(x => x)
                .Must(x =>
                {
                    if (string.IsNullOrEmpty(x.Phone))
                    {
                        return true;
                    }
                    return SmsUtility.IsVietnamesePhone(x.Phone);
                })
                .WithMessage(localizer["Authentication.PhoneIsNotValid"]);
        }
    }
}
