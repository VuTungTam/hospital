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

        public static List<string> DefaultRandomPassword = new List<string>
        {
            "XukaYeuChaien",
            "NobitaDiHonda",
            "NobitaChaXeko",
            "DekhiYeuMimi",
            "DoremonDiLonTon",
            "ChaienBeNho",
            "XukaMoNhon"
        };

        public async Task AddDoctorAsync(Doctor doctor, CancellationToken cancellationToken = default)
        {
            var sequenceRepository = _serviceProvider.GetRequiredService<ISequenceRepository>();
            var table = new Doctor().GetTableName();
            var code = await sequenceRepository.GetSequenceAsync(table, cancellationToken);

            if (string.IsNullOrEmpty(doctor.Password))
            {
                var random = new Random();
                doctor.Password = DefaultRandomPassword[random.Next(0, DefaultRandomPassword.Count)];
                doctor.IsDefaultPassword = true;
                doctor.IsPasswordChangeRequired = true;
            }
            doctor.Code = code.ValueString;
            doctor.HashPassword();
            doctor.FacilityId = _executionContext.FacilityId;
            doctor.LastSeen = null;

            await _dbSet.AddAsync(doctor, cancellationToken);
        }

        public async Task UpdateSpecialtiesAsync(long doctorId, IEnumerable<long> specialtyIds, CancellationToken cancellationToken)
        {
            var sql = $"DELETE FROM {new DoctorSpecialty().GetTableName()} WHERE {nameof(DoctorSpecialty.DoctorId)} = {doctorId}; ";
            foreach (var specialtyId in specialtyIds)
            {
                sql += $"INSERT INTO {new DoctorSpecialty().GetTableName()}(Id,SpecialtyId, {nameof(DoctorSpecialty.DoctorId)}, CreatedBy, CreatedAt) VALUES ({AuthUtility.GenerateSnowflakeId()},{specialtyId}, {doctorId}, {_executionContext.Identity}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'); ";
            }

            await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken);
        }
    }

}
