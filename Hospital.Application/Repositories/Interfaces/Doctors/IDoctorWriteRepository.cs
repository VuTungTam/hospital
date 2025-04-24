using Hospital.Application.Dtos.Doctors;
using Hospital.Domain.Entities.Doctors;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Doctors
{
    public interface IDoctorWriteRepository : IWriteRepository<Doctor>
    {
        Task AddDoctorAsync(Doctor doctor, CancellationToken cancellationToken = default);

        Task UpdateDoctorAsync(Doctor doctor, DoctorDto newdoctor, CancellationToken cancellationToken = default);

        Task UpdateSpecialtiesAsync(long doctorId, IEnumerable<long> specialtyIds, CancellationToken cancellationToken);
    }
}
