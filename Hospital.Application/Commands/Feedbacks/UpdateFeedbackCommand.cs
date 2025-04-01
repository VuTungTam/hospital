using Hospital.Application.Dtos.Feedbacks;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Feedbacks
{
    [RequiredPermission(ActionExponent.UpdateFeedback)]
    public class UpdateFeedbackCommand : BaseCommand
    {
        public UpdateFeedbackCommand(FeedbackDto feedback) 
        {
            Feedback = feedback;
        }
        public FeedbackDto Feedback { get; }
    }

}
