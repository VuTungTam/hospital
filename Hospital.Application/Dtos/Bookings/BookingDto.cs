﻿using FluentValidation;
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

        public string SpecialtyNameVn { get; set; }

        public string SpecialtyNameEn { get; set; }

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
            RuleFor(x => x.ServiceId)
                .NotEmpty().WithMessage(localizer["Booking.ServiceIsRequired"])
                .Must(x => long.TryParse(x, out var id) && id > 0)
                .WithMessage(localizer["Booking.ServiceIsNotValid"]);

            RuleFor(x => x.HealthProfileId)
                .NotEmpty().WithMessage(localizer["Booking.HealthProfileIsRequired"])
                .Must(x => long.TryParse(x, out var id) && id > 0)
                .WithMessage(localizer["Booking.HealthProfileIsNotValid"]);

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage(localizer["Booking.DateIsRequired"])
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage(localizer["Booking.DateMustBeInFuture"]);

            RuleFor(x => x.TimeSlotId)
                .NotEmpty().WithMessage(localizer["Booking.TimeSlotIsRequired"]);
        }
    }
}