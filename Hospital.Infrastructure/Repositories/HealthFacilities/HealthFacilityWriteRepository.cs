using System.Text;
using AutoMapper;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.Images;
using Hospital.Domain.Entities.Specialties;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthFacilities
{
    public class HealthFacilityWriteRepository : WriteRepository<HealthFacility>, IHealthFacilityWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILocationReadRepository _locationReadRepository;
        public HealthFacilityWriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache,
            ILocationReadRepository locationReadRepository,
            IMapper mapper
            ) : base(serviceProvider, localizer, redisCache)
        {
            _mapper = mapper;
            _locationReadRepository = locationReadRepository;
        }
        public async Task RemoveFacilitySpecialtyAsync(FacilitySpecialty facilitySpecialty, CancellationToken cancellationToken)
        {
            var dateService = _serviceProvider.GetRequiredService<IDateService>();
            var clientTime = dateService.GetClientTime();
            var tableName = new FacilitySpecialty().GetTableName();
            var sql = $"UPDATE {tableName} SET Deleted = @ClientTime WHERE Id = @FacilitySpecialtyId";
            await _dbContext.Database.ExecuteSqlRawAsync(sql,
                new[]
                {
                new SqlParameter("@ClientTime", clientTime),
                new SqlParameter("@FacilitySpecialtyId", facilitySpecialty.Id)
                },
                cancellationToken: cancellationToken);
            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
        }

        public async Task UpdateFacilityAsync(HealthFacility oldFacility, HealthFacilityDto newFacility, CancellationToken cancellationToken)
        {
            oldFacility.NameVn = newFacility.NameVn;

            oldFacility.NameEn = newFacility.NameEn;

            oldFacility.DescriptionVn = newFacility.DescriptionVn;

            oldFacility.DescriptionEn = newFacility.DescriptionEn;

            oldFacility.SummaryVn = newFacility.SummaryVn;

            oldFacility.SummaryEn = newFacility.SummaryEn;

            oldFacility.Logo = newFacility.Logo;

            oldFacility.Wid = int.Parse(newFacility.Wid);

            oldFacility.Did = int.Parse(newFacility.Did);

            oldFacility.Pid = int.Parse(newFacility.Pid);

            oldFacility.Address = newFacility.Address;

            oldFacility.MapURL = newFacility.MapURL;

            oldFacility.Wname = await _locationReadRepository.GetNameByIdAsync(int.Parse(newFacility.Wid), "ward", cancellationToken);

            oldFacility.Dname = await _locationReadRepository.GetNameByIdAsync(int.Parse(newFacility.Did), "district", cancellationToken);

            oldFacility.Pname = await _locationReadRepository.GetNameByIdAsync(int.Parse(newFacility.Pid), "province", cancellationToken);

            var oldImageNames = oldFacility.Images.Select(x => x.PublicId.Split('/').Last()).ToList();
            var newImageNames = newFacility.ImageNames ?? new List<string>();
            var delImages = oldFacility.Images.Where(i => !newImageNames.Contains(i.PublicId.Split('/').Last())).ToList();
            var addImages = newImageNames.Except(oldImageNames).ToList();

            _dbContext.Images.RemoveRange(delImages);
            foreach (var img in addImages)
            {
                _dbContext.Images.Add(new Image { Id = AuthUtility.GenerateSnowflakeId(), PublicId = img, FacilityId = oldFacility.Id });
            }

            var oldSpecs = oldFacility.FacilitySpecialties.Select(x => x.SpecialtyId.ToString()).ToList();
            var newSpecs = newFacility.SpecialtyIds;
            var delSpecs = oldFacility.FacilitySpecialties.Where(s => !newSpecs.Contains(s.SpecialtyId.ToString())).ToList();
            var addSpecs = newSpecs.Except(oldSpecs).ToList();

            _dbContext.FacilitySpecialties.RemoveRange(delSpecs);
            foreach (var spec in addSpecs)
            {
                _dbContext.FacilitySpecialties.Add(new FacilitySpecialty { Id = AuthUtility.GenerateSnowflakeId(), FacilityId = oldFacility.Id, SpecialtyId = long.Parse(spec) });
            }

            var oldTypes = oldFacility.FacilityTypeMappings.Select(x => x.TypeId.ToString()).ToList();
            var newTypes = newFacility.TypeIds;
            var delTypes = oldFacility.FacilityTypeMappings.Where(t => !newTypes.Contains(t.TypeId.ToString())).ToList();
            var addTypes = newTypes.Except(oldTypes).ToList();

            _dbContext.FacilityTypeMappings.RemoveRange(delTypes);
            foreach (var type in addTypes)
            {
                _dbContext.FacilityTypeMappings.Add(new FacilityTypeMapping { FacilityId = oldFacility.Id, TypeId = long.Parse(type) });
            }

            await UpdateAsync(oldFacility, cancellationToken: cancellationToken);
        }

    }
}