using Hospital.Domain.Entities.Feedbacks;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Feedbacks
{
    public interface IFeedbackWriteRepository : IWriteRepository<Feedback>
    {
    }
}
