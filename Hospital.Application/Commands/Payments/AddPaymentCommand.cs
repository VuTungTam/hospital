using Hospital.Application.Dtos.Payments;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Payments
{
    public class AddPaymentCommand : BaseCommand<string>
    {
        public AddPaymentCommand(PaymentDto dto)
        {
            Dto = dto;
        }
        public PaymentDto Dto { get; set; }
    }
}
