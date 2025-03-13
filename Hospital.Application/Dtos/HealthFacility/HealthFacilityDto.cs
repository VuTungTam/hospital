using FluentValidation;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Infrastructure.Services.Sms.Utils;
using Hospital.SharedKernel.Infrastructure.Services.Websites.Utils;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.HealthFacility
{
    public class HealthFacilityDto : BaseDto
    {
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string DescriptionVn { get; set; }
        public string DescriptionEn { get; set; }
        public string ImageUrl { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public long CategoryId { get; set; }
        public HealthFacilityStatus Status { get; set; }
        public string StatusText => Status.GetDescription();
        public string Pid { get; set; }
        public string Pname { get; set; }
        public string Did { get; set; }
        public string Dname { get; set; }
        public string Wid { get; set; }
        public string Wname { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }
    }
    public class HealthFacilityValidator : BaseAbstractValidator<HealthFacilityDto>
    {
        public HealthFacilityValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.NameVn).NotEmpty().WithMessage(localizer["health_facility_name_vn_is_not_empty"]);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage(localizer["health_facility_name_en_is_not_empty"]);
            RuleFor(x => x.DescriptionVn).NotEmpty().WithMessage(localizer["health_facility_description_vn_is_not_empty"]);
            RuleFor(x => x.DescriptionEn).NotEmpty().WithMessage(localizer["health_facility_description_en_is_not_empty"]);
            RuleFor(x => x.Address).NotEmpty().WithMessage(localizer["health_facility_address_is_not_empty"]);
            RuleFor(x => x.Phone).Must(x => SmsUtility.IsVietnamesePhone(x)).WithMessage("invalid_phone_number");
            RuleFor(x => x.Email).Must(x => EmailUtility.IsEmail(x)).WithMessage(localizer["invalid_email"]);
            RuleFor(x => x.Website).Must(x => WebsiteUtility.BeAValidUrl(x)).WithMessage(localizer["invalid_website_url"]);
            RuleFor(x => x.Pid).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_province"]);
            RuleFor(x => x.Did).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_district"]);
            RuleFor(x => x.Wid).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_ward"]);
        }
    }
}
