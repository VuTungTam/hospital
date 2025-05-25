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

        public DateTime? ExpiredAt { get; set; }

        public class PaymentValidator : BaseAbstractValidator<PaymentDto>
        {
            public PaymentValidator(IStringLocalizer<Resources> localizer) : base(localizer)
            {
                RuleFor(x => x.Amount)
                    .GreaterThan(0).WithMessage(localizer["Số tiền phải lớn hơn 0."]);

                RuleFor(x => x.BookingId)
                .NotEmpty().WithMessage(localizer["Bookings.ServiceIsRequired"])
                .Must(x => long.TryParse(x, out var id) && id > 0)
                .WithMessage(localizer["Bookings.ServiceIsNotValid"]);

                RuleFor(x => x.PaymentMethod)
                    .IsInEnum().WithMessage(localizer["Phương thức thanh toán không hợp lệ."]);

                RuleFor(x => x.TransactionContent)
                    .MaximumLength(255).WithMessage(localizer["Nội dung giao dịch không được quá 255 ký tự."]);

                RuleFor(x => x.ExpiredAt)
                    .GreaterThanOrEqualTo(DateTime.Now).When(x => x.ExpiredAt.HasValue)
                    .WithMessage(localizer["Ngày hết hạn phải sau ngày hiện tại."]);
            }
        }
    }
}
