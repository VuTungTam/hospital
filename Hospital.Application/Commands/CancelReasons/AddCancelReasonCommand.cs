using Hospital.Application.Dtos.CancelReasons;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Feedbacks
{
    [RequiredPermission(ActionExponent.CancelBooking)]
    public class AddCancelReasonCommand : BaseCommand<string>
    {
        public AddCancelReasonCommand(CancelReasonDto dto)
        {
            Dto = dto;
        }
        public CancelReasonDto Dto { get; set; }
    }
}
