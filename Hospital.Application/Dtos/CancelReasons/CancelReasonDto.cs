using FluentValidation;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.CancelReasons
{
    public class CancelReasonDto : BaseDto
    {
        public string Reason { get; set; }

        public string BookingId { get; set; }

        public CancelType CancelType { get; set; }
    }
    public class BookingValidator : BaseAbstractValidator<CancelReasonDto>
    {
        public BookingValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            // Validate ServiceId
            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage(localizer["CancelReason.ReasonIsRequired"]);

            // Validate HealthProfileId
            RuleFor(x => x.BookingId)
                .NotEmpty().WithMessage(localizer["CancelReason.BookingIsRequired"])
                .Must(x => long.TryParse(x, out var id) && id > 0)
                .WithMessage(localizer["CancelReason.BookingIsNotValid"]);

            // Validate TimeSlotId
            RuleFor(x => x.CancelType)
                .NotEmpty().WithMessage(localizer["CancelReason.CancelTypeIsRequired"]);
        }
    }
}