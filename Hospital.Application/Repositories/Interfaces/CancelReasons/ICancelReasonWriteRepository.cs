using Hospital.Domain.Entities.CancelReasons;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.CancelReasons
{
    public interface ICancelReasonWriteRepository : IWriteRepository<CancelReason>
    {

    }
}