using FluentValidation;
using Hospital.Application.Dtos.Symptoms;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Bookings
{
    public class BookingResponseDto : BaseDto
    {
        public string Code { get; set; }

        public string HealthProfileId { get; set; }

        public BookingStatus Status { get; set; }

        public string StatusDescription => Status.GetDescription();

        public DateTime Date { get; set; }

        public string ServiceId { get; set; }

        public string ServiceNameVn { get; set; }

        public string ServiceNameEn { get; set; }

        public TimeSpan ServiceStartTime { get; set; }

        public TimeSpan ServiceEndTime { get; set; }

        public int Order { get; set; }

        public string OwnerId { get; set; }

        public List<string> SymptomIds { get; set; } 

        public List<string> SymptomNameVns { get; set; }

        public List<string> SymptomNameEns { get; set; }
    }
    public class BookingValidator : BaseAbstractValidator<BookingResponseDto>
    {
        public BookingValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            //RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["Bookings.NameMustNotBeEmpty"]);
            //RuleFor(x => x.Phone).Must(x => SmsUtility.IsVietnamesePhone(x)).WithMessage(localizer["Bookings.PhoneIsNotValid"]);
            RuleFor(x => x.ServiceId).Must(x => long.TryParse(x, out var id) && id > 0).WithMessage(localizer["Bookings.ServiceIsNotValid"]);
        }
    }
}