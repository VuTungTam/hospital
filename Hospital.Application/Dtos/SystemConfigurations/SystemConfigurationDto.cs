using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.SystemConfigurations
{
    public class SystemConfigurationDto
    {
        public string Id { get; set; }

        public bool IsEnabledVerifiedAccount { get; set; }

        public int RequiresPasswordLevel { get; set; }

        public int Session { get; set; }

        public int PasswordMinLength { get; set; }

        public int MaxNumberOfSmsPerDay { get; set; }

        public bool PreventCopying { get; set; }

        public string BookingNotificationEmail { get; set; }

        public List<string> BookingNotificationBccEmails { get; set; }
    }

    public class SystemConfigurationDtoValidator : BaseAbstractValidator<SystemConfigurationDto>
    {
        public SystemConfigurationDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Session)
                .Must(s => (s >= 86400 && s <= 86400 * 7) && (s % 86400 == 0))
                .WithMessage("Thời gian của một phiên làm việc không hợp lệ");

            RuleFor(x => x.PasswordMinLength)
                .Must(l => l >= 6 && l <= 64)
                .WithMessage("Độ dài mật khẩu không hợp lệ");

            RuleFor(x => x.MaxNumberOfSmsPerDay)
                .Must(l => l >= 1 && l <= 500)
                .WithMessage("Số lượng SMS không hợp lệ");

            RuleFor(x => x.BookingNotificationEmail)
                .Must(x => string.IsNullOrEmpty(x) || EmailUtility.IsEmail(x))
                .WithMessage("Email nhận thông báo đặt lịch không hợp lệ");

            RuleFor(x => x.BookingNotificationBccEmails)
                .Must(emails => emails == null || emails.All(x => EmailUtility.IsEmail(x)))
                .WithMessage("Danh sách BCC nhận thông báo đặt lịch chứa email không hợp lệ");
        }
    }
}
