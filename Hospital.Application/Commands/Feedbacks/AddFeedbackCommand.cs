using Hospital.Application.Dtos.Feedbacks;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Feedbacks
{
    public class AddFeedbackCommand : BaseCommand<string>
    {
        public AddFeedbackCommand(FeedbackDto dto)
        {
            Dto = dto;
        }
        public FeedbackDto Dto { get; set; }
    }
}
