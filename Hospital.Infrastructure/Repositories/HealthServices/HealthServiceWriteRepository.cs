using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Utils;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.HealthServices
{
    public class HealthServiceWriteRepository : WriteRepository<HealthService>, IHealthServiceWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly IServiceTimeRuleWriteRepository _serviceTimeRuleWriteRepository;
        public HealthServiceWriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache,
            IServiceTimeRuleWriteRepository serviceTimeRuleWriteRepository,
            IMapper mapper
            ) : base(serviceProvider, localizer, redisCache)
        {
            _mapper = mapper;
            _serviceTimeRuleWriteRepository = serviceTimeRuleWriteRepository;
        }

        public async Task UpdateServiceAsync(HealthService oldEntity, HealthServiceDto newEntity, CancellationToken cancellationToken)
        {
            oldEntity.NameVn = newEntity.NameVn;
            oldEntity.NameEn = newEntity.NameEn;
            oldEntity.TypeId = long.Parse(newEntity.TypeId);
            oldEntity.SpecialtyId = long.Parse(newEntity.SpecialtyId);
            oldEntity.DoctorId = long.Parse(newEntity.DoctorId);
            oldEntity.Status = newEntity.Status;
            oldEntity.Price = newEntity.Price;
            Update(oldEntity);
            var oldTrs = oldEntity.ServiceTimeRules.ToList();
            var newTrs = _mapper.Map<List<ServiceTimeRule>>(newEntity.ServiceTimeRules);

            var delTrs = oldTrs.Except(newTrs).ToList();
            var intersectTrs = oldTrs.Intersect(newTrs).ToList();
            var addTrs = newTrs.Except(oldTrs).ToList();

            foreach (var existingRule in intersectTrs)
            {
                var updatedData = newTrs.FirstOrDefault(r => r.Equals(existingRule));
                if (updatedData != null)
                {
                    existingRule.MaxPatients = updatedData.MaxPatients;
                }
            }

            foreach (var timeRule in delTrs)
            {
                var removeSlots = _dbContext.TimeSlots.Where(x => x.TimeRuleId == timeRule.Id);
                _dbContext.TimeSlots.RemoveRange(removeSlots);
            }
            _dbContext.ServiceTimeRules.RemoveRange(delTrs);

            foreach (var newTimeRule in addTrs)
            {
                newTimeRule.Id = AuthUtility.GenerateSnowflakeId();
                newTimeRule.ServiceId = oldEntity.Id;

                _dbContext.ServiceTimeRules.Add(newTimeRule);

                var timeSlots = _serviceTimeRuleWriteRepository.GenerateTimeSlots(newTimeRule);
                _dbContext.TimeSlots.AddRange(timeSlots);
            }

            await UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
        }
    }
}
