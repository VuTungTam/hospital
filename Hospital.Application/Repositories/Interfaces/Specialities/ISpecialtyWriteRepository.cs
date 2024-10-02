using Hospital.Domain.Entities.Specialties;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Specialities
{
    public interface ISpecialtyWriteRepository : IWriteRepository<Specialty>
    {
    }
}
