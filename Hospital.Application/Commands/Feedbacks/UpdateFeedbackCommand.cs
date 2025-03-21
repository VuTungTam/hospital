using Hospital.Application.Dtos.Feedbacks;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Feedbacks
{
    public class UpdateFeedbackCommand : BaseCommand
    {
        public UpdateFeedbackCommand(FeedbackDto feedback) 
        {
            Feedback = feedback;
        }
        public FeedbackDto Feedback { get; }
    }

}
