using FluentValidation;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Payments
{
    public class PaymentDto : BaseDto
    {
        public string BookingId { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public decimal Amount { get; set; }

        public string TransactionContent { get; set; }

        public string TransactionId { get; set; }

        public string FacilityId { get; set; }

        public class PaymentValidator : BaseAbstractValidator<PaymentDto>
        {
            public PaymentValidator(IStringLocalizer<Resources> localizer) : base(localizer)
            {
                RuleFor(x => x.Amount)
                    .GreaterThan(0).WithMessage(localizer["Payment.AmountMustBeGreaterThanZero"]);

                RuleFor(x => x.BookingId)
                    .NotEmpty().WithMessage(localizer["Payment.BookingIsRequired"])
                    .Must(x => long.TryParse(x, out var id) && id > 0)
                    .WithMessage(localizer["Payment.BookingIsNotValid"]);

                RuleFor(x => x.PaymentMethod)
                    .IsInEnum().WithMessage(localizer["Payment.PaymentMethodIsInvalid"]);

                RuleFor(x => x.TransactionContent)
                    .MaximumLength(255).WithMessage(localizer["Payment.TransactionContentMaxLength"]);
            }
        }
    }
}
