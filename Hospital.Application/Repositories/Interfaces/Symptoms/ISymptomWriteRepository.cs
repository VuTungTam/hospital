using Hospital.Domain.Entities.Symptoms;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Symptoms
{
    public interface ISymptomWriteRepository : IWriteRepository<Symptom>
    {
    }
}
