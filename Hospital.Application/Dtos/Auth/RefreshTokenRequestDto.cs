using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Auth
{
    public class RefreshTokenRequestDto
    {
        public long UserId { get; set; }

        public string RefreshToken { get; set; }
    }

    public class RefreshTokenRequestDtoValidator : BaseAbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage(localizer["auth_userid_must_not_be_empty"].Value);
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage(localizer["auth_refresh_token_must_not_be_empty"].Value);
        }
    }
}
