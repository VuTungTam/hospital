using FluentValidation;
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

        public string HealthProfileName { get; set; }

        public BookingStatus Status { get; set; }

        public string StatusDescription => Status.GetDescription();

        public DateTime Date { get; set; }

        public string ServiceId { get; set; }

        public string ServiceTypeId { get; set; }

        public string ServiceTypeNameVn { get; set; }

        public string ServiceTypeNameEn { get; set; }

        public string ServiceNameVn { get; set; }

        public string ServiceNameEn { get; set; }

        public string TimeSlotId { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string TimeRange { get; set; }

        public TimeSpan StartBooking { get; set; }

        public TimeSpan EndBooking { get; set; }

        public string Order { get; set; }

        public bool IsFeedbacked { get; set; } = false;

        public TimeSpan ServiceStartTime { get; set; }

        public TimeSpan ServiceEndTime { get; set; }

        public string OwnerId { get; set; }

        public string FacilityId { get; set; }

        public string SpecialtyId { get; set; }

        public string FacilityNameVn { get; set; }

        public string FacilityNameEn { get; set; }

        public string FacilityFullAddress { get; set; }

        public string ZoneId { get; set; }

        public string DoctorId { get; set; }

    }
    public class BookingValidator : BaseAbstractValidator<BookingDto>
    {
        public BookingValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            // Validate ServiceId
            RuleFor(x => x.ServiceId)
                .NotEmpty().WithMessage(localizer["Bookings.ServiceIsRequired"])
                .Must(x => long.TryParse(x, out var id) && id > 0)
                .WithMessage(localizer["Bookings.ServiceIsNotValid"]);

            // Validate HealthProfileId
            RuleFor(x => x.HealthProfileId)
                .NotEmpty().WithMessage(localizer["Bookings.HealthProfileIsRequired"])
                .Must(x => long.TryParse(x, out var id) && id > 0)
                .WithMessage(localizer["Bookings.HealthProfileIsNotValid"]);

            // Validate Date
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage(localizer["Bookings.DateIsRequired"])
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage(localizer["Bookings.DateMustBeInFuture"]);

            // Validate TimeSlotId
            RuleFor(x => x.TimeSlotId)
                .NotEmpty().WithMessage(localizer["Bookings.TimeSlotIsRequired"]);
        }
    }
}