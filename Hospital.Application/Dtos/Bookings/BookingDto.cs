using FluentValidation;
using Hospital.Application.Dtos.Symptoms;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Bookings
{
    public class BookingDto : BaseDto
    {
        public string Code { get; set; }

        public string HealthProfileId { get; set; }

        public BookingStatus Status { get; set; }

        public string StatusDescription => Status.GetDescription();

        public DateTime Date { get; set; }

        public string ServiceId { get; set; }

        public string ServiceNameVn { get; set; }

        public string ServiceNameEn { get; set; }

        public string TimeSlotId { get; set; }

        public TimeSpan StartBooking { get; set; }

        public TimeSpan EndBooking { get; set; }

        public int Order { get; set; }

        public bool IsFeedbacked { get; set; } = false;

        public TimeSpan ServiceStartTime { get; set; }

        public TimeSpan ServiceEndTime { get; set; }

        public string OwnerId { get; set; }

        public string FacilityId { get; set; }

        public string ZoneId { get; set; }

        public string DoctorId { get; set; }

        public List<SymptomDto> Symptoms { get; set; }

        public List<string> SymptomIds { get; set; } 

        public List<string> SymptomNameVns { get; set; }

        public List<string> SymptomNameEns { get; set; }
    }
    public class BookingValidator : BaseAbstractValidator<BookingDto>
    {
        public BookingValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            //RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["Bookings.NameMustNotBeEmpty"]);
            //RuleFor(x => x.Phone).Must(x => SmsUtility.IsVietnamesePhone(x)).WithMessage(localizer["Bookings.PhoneIsNotValid"]);
            RuleFor(x => x.ServiceId).Must(x => long.TryParse(x, out var id) && id > 0).WithMessage(localizer["Bookings.ServiceIsNotValid"]);
            RuleFor(x => x.ZoneId).Must(x => int.TryParse(x, out var id) && id >= 0).WithMessage(localizer["invalid_zone_id"]);
            RuleFor(x => x.FacilityId).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_facility_id"]);
            RuleFor(x => x.DoctorId).Must(x => int.TryParse(x, out var id) && id >= 0).WithMessage(localizer["invalid_doctor_id"]);
            RuleFor(x => x.OwnerId).Must(x => int.TryParse(x, out var id) && id > 0).WithMessage(localizer["invalid_owner_id"]);
        }
    }
}