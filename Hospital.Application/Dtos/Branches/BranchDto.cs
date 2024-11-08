using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Infrastructure.Services.Sms.Utils;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Branches
{
    public class BranchDto : BaseDto
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public DateTime? FoundingDate { get; set; }

        public bool Active { get; set; }

        public bool HasPermission { get; set; } = true;
    }

    public class BranchDtoValidator : BaseAbstractValidator<BranchDto>
    {
        public BranchDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizer["branch_name_must_not_be_empty"]);

            RuleFor(x => x.Phone)
                .Must(p => SmsUtility.IsVietnamesePhone(p))
                .WithMessage(localizer["branch_phone_is_not_valid"]);

            RuleFor(x => x.Email)
                .Must(e => EmailUtility.IsEmail(e))
                .WithMessage(localizer["branch_email_is_not_valid"]);

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage(localizer["branch_address_must_not_be_empty"]);
        }
    }
}
