using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Entities.Specialties;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Doctors
{
    public class DoctorWriteRepository : WriteRepository<Doctor>, IDoctorWriteRepository
    {
        public DoctorWriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
            ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task AddDoctorAsync(Doctor doctor, CancellationToken cancellationToken = default)
        {
            var sequenceRepository = _serviceProvider.GetRequiredService<ISequenceRepository>();
            var table = new Doctor().GetTableName();
            var code = await sequenceRepository.GetSequenceAsync(table, cancellationToken);

            if (string.IsNullOrEmpty(doctor.Password))
            {
                var random = new Random();
                doctor.Password = "doctor@1";
                doctor.IsDefaultPassword = true;
                doctor.IsPasswordChangeRequired = true;
            }
            doctor.Code = code.ValueString;
            doctor.HashPassword();
            doctor.FacilityId = _executionContext.FacilityId;
            doctor.LastSeen = null;

            _dbSet.Add(doctor);
        }
        public async Task UpdateDoctorAsync(Doctor doctor, DoctorDto newDoctor, CancellationToken cancellationToken = default)
        {
            var oldSpecs = doctor.DoctorSpecialties.Select(x => x.SpecialtyId.ToString()).ToList();
            var newSpecs = newDoctor.SpecialtyIds;
            var delSpecs = doctor.DoctorSpecialties.Where(s => !newSpecs.Contains(s.SpecialtyId.ToString())).ToList();
            var addSpecs = newSpecs.Except(oldSpecs).ToList();

            _dbContext.DoctorSpecialties.RemoveRange(delSpecs);
            foreach (var spec in addSpecs)
            {
                _dbContext.DoctorSpecialties.Add(new DoctorSpecialty
                {
                    Id = AuthUtility.GenerateSnowflakeId(),
                    DoctorId = doctor.Id,
                    SpecialtyId = long.Parse(spec)
                });
            }


            await UpdateAsync(doctor, cancellationToken: cancellationToken);
        }

        public async Task UpdateSpecialtiesAsync(long doctorId, IEnumerable<long> specialtyIds, CancellationToken cancellationToken)
        {
            var sql = $"DELETE FROM {new DoctorSpecialty().GetTableName()} WHERE {nameof(DoctorSpecialty.DoctorId)} = {doctorId}; ";

            foreach (var specialtyId in specialtyIds)
            {
                sql += $"INSERT INTO {new DoctorSpecialty().GetTableName()}(Id, SpecialtyId, {nameof(DoctorSpecialty.DoctorId)}) " +
                       $"VALUES ({AuthUtility.GenerateSnowflakeId()}, {specialtyId}, {doctorId}); ";
            }

            await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken);
        }
    }
}
