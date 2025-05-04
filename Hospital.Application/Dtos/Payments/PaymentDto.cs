using FluentValidation;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Payments
{
    public class PaymentDto : BaseDto
    {
        public long BookingId { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public decimal Amount { get; set; }

        public string TransactionContent { get; set; }

        public string PaymentUrl { get; set; }

        public bool IsPaid { get; set; }

        public string ExternalTransactionId { get; set; }

        public string BankBin { get; set; }

        public string Note { get; set; }

        public string FacilityId { get; set; }

        public DateTime? ExpiredAt { get; set; }

        public class PaymentValidator : BaseAbstractValidator<PaymentDto>
        {
            public PaymentValidator(IStringLocalizer<Resources> localizer) : base(localizer)
            {
                RuleFor(x => x.Amount)
                    .GreaterThan(0).WithMessage(localizer["Số tiền phải lớn hơn 0."]);

                RuleFor(x => x.BookingId)
                    .GreaterThan(0).WithMessage(localizer["BookingId phải hợp lệ."]);

                RuleFor(x => x.PaymentMethod)
                    .IsInEnum().WithMessage(localizer["Phương thức thanh toán không hợp lệ."]);

                RuleFor(x => x.TransactionContent)
                    .MaximumLength(255).WithMessage(localizer["Nội dung giao dịch không được quá 255 ký tự."]);

                RuleFor(x => x.PaymentUrl)
                    .Matches(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$").When(x => !string.IsNullOrEmpty(x.PaymentUrl))
                    .WithMessage(localizer["Đường dẫn thanh toán không hợp lệ."]);

                RuleFor(x => x.ExpiredAt)
                    .GreaterThanOrEqualTo(DateTime.Now).When(x => x.ExpiredAt.HasValue)
                    .WithMessage(localizer["Ngày hết hạn phải sau ngày hiện tại."]);
            }
        }
    }
}
