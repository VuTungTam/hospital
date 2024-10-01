using Hospital.Domain.Entities.Visits;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Visits
{
    public interface IVisitWriteRepository : IWriteRepository<Visit>
    {
    }
}
