using Hospital.Application.Dtos.Feedbacks;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Feedbacks
{
    [RequiredPermission(ActionExponent.AddFeedback)]
    public class AddFeedbackCommand : BaseCommand<string>
    {
        public AddFeedbackCommand(FeedbackDto dto)
        {
            Dto = dto;
        }
        public FeedbackDto Dto { get; set; }
    }
}
