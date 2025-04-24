using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Configures.Models;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Infrastructure.Services.Sms.Utils;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Users
{
    public class BaseUserDto : BaseDto
    {
        public string Code { get; set; }

        public string Phone { get; set; }

        public bool PhoneVerified { get; set; }

        public string Email { get; set; }

        public bool EmailVerified { get; set; }

        public string Name { get; set; }

        public string ShortName
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return "";
                }

                var words = Name.Trim().Split(" ");
                if (words.Length == 1)
                {
                    return Name.First().ToString().ToUpper();
                }
                return (words[0].First().ToString() + words[words.Length - 1].First().ToString()).ToUpper();
            }
        }

        public string BackgroundColor
        {
            get
            {
                if (string.IsNullOrEmpty(ShortName))
                {
                    return "#fff";
                }

                var c = ShortName[0];
                if (c <= 70)
                {
                    return "#F8881F";
                }
                else if (c <= 75)
                {
                    return "#dd342f";
                }
                else if (c <= 80)
                {
                    return "#eb4d83";
                }
                else if (c <= 95)
                {
                    return "#6d6d6d";
                }
                return "#5D4037";
            }
        }

        public DateTime? Dob { get; set; }

        public int Gender { get; set; }

        public string Pid { get; set; }

        public string Pname { get; set; }

        public string Did { get; set; }

        public string Dname { get; set; }

        public string Wid { get; set; }

        public string Wname { get; set; }

        public string Address { get; set; }

        public AccountStatus Status { get; set; }

        public string Provider { get; set; }

        // Nếu có thì hiển thị thay cho PhotoUrl
        public string Avatar { get; set; }

        public string AvatarUrl => !string.IsNullOrEmpty(Avatar) ? CdnConfig.Get(Avatar) : (!string.IsNullOrEmpty(PhotoUrl) ? PhotoUrl : "");

        public string PhotoUrl { get; set; }

        public int Shard { get; set; }

        public bool CanChangePassword { get; set; }

        public bool IsDefaultPassword { get; set; }

        public bool IsPasswordChangeRequired { get; set; }

        public DateTime? LastSeen { get; set; }

        public UserLocation Location => new UserLocation
        {
            Pid = Pid,
            Did = Did,
            Wid = Wid,
            Pname = Pname,
            Dname = Dname,
            Wname = Wname,
            Address = Address,
        };

        public UserSystemInfo SystemInfo => new UserSystemInfo
        {
            CanChangePassword = CanChangePassword,
            IsPasswordChangeRequired = IsPasswordChangeRequired,
            IsDefaultPassword = IsDefaultPassword,
            EmailVerified = EmailVerified,
            LastSeen = LastSeen,
            PhoneVerified = PhoneVerified,
            Provider = Provider,
            Shard = Shard,
            Status = Status,
            CreatedAt = CreatedAt,
            ModifiedAt = ModifiedAt
        };
    }

    public class UserLocation
    {
        public string Pid { get; set; }
        public string Did { get; set; }
        public string Wid { get; set; }
        public string Pname { get; set; }
        public string Dname { get; set; }
        public string Wname { get; set; }
        public string Address { get; set; }
    }

    public class UserSystemInfo
    {
        public AccountStatus Status { get; set; }
        public string Provider { get; set; }
        public bool PhoneVerified { get; set; }
        public bool CanChangePassword { get; set; }
        public bool IsDefaultPassword { get; set; }
        public bool IsPasswordChangeRequired { get; set; }
        public bool EmailVerified { get; set; }
        public DateTime? LastSeen { get; set; }
        public int Shard { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class BaseUserDtoDtoValidator<T> : BaseAbstractValidator<T> where T : BaseUserDto
    {
        public BaseUserDtoDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            //RuleFor(x => x.Code).NotEmpty().WithMessage("Mã không được để trống");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống");
            //RuleFor(x => x.Dob).Must(x => x != default && x < DateTime.Now && x > new DateTime(1950, 1, 1)).WithMessage("Ngày sinh không hợp lệ");
            RuleFor(x => x.Phone).Must(x => SmsUtility.IsVietnamesePhone(x)).WithMessage("Số điện thoại không hợp lệ");
            RuleFor(x => x.Email).Must(x => EmailUtility.IsEmail(x)).WithMessage("Email không hợp lệ");
            //RuleFor(x => x.Pid).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage("Tỉnh/thành không hợp lệ");
            //RuleFor(x => x.Did).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage("Quận/huyện không hợp lệ");
            //RuleFor(x => x.Wid).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage("Xã/phường không hợp lệ");
        }
    }
}
