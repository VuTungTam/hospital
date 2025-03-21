using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Models
{
    public class UserPwdModel
    {
        public long UserId { get; set; }
        public string NewPassword { get; set; }
    }

    public class UserPwdModelValidator : BaseAbstractValidator<UserPwdModel>
    {
        public UserPwdModelValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.UserId).Must(x => x > 0).WithMessage("Id không hợp lệ");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Mật khẩu không được để trống");
        }
    }
}
