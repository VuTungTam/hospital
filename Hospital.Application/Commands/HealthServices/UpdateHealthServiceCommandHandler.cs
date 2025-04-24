using AutoMapper;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealthServices
{
    public class UpdateHealthServiceCommandHandler : BaseCommandHandler, IRequestHandler<UpdateHealthServiceCommand>
    {
        private readonly IHealthServiceWriteRepository _healthServiceWriteRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly IServiceTimeRuleWriteRepository _serviceTimeRuleWriteRepository;
        private readonly ITimeSlotWriteRepository _timeSlotWriteRepository;
        public UpdateHealthServiceCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IHealthServiceWriteRepository healthServiceWriteRepository,
            IHealthServiceReadRepository healthServiceReadRepository,
            IServiceTimeRuleWriteRepository serviceTimeRuleWriteRepository,
            ITimeSlotWriteRepository timeSlotWriteRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _healthServiceWriteRepository = healthServiceWriteRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
            _serviceTimeRuleWriteRepository = serviceTimeRuleWriteRepository;
            _timeSlotWriteRepository = timeSlotWriteRepository;
        }


        public async Task<Unit> Handle(UpdateHealthServiceCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.HealthService.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var option = new QueryOption
            {
                Includes = new string[] { nameof(HealthService.ServiceTimeRules) }
            };

            var oldService = await _healthServiceReadRepository.GetByIdAsync(id, option, cancellationToken);

            if (oldService == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            var newService = _mapper.Map<HealthService>(request.HealthService);

            _serviceTimeRuleWriteRepository.Delete(oldService.ServiceTimeRules);
            oldService.ServiceTimeRules.Clear();

            oldService.ServiceTimeRules = newService.ServiceTimeRules
                .Select(rule =>
                {
                    rule.Id = AuthUtility.GenerateSnowflakeId();
                    rule.ServiceId = oldService.Id;
                    return rule;
                }).ToList();

            oldService.NameVn = newService.NameVn;
            oldService.NameEn = newService.NameEn;
            oldService.DescriptionVn = newService.DescriptionVn;
            oldService.DescriptionEn = newService.DescriptionEn;
            oldService.TypeId = newService.TypeId;
            oldService.Price = newService.Price;

            _healthServiceWriteRepository.Update(oldService);

            foreach (var timeRule in oldService.ServiceTimeRules)
            {
                var timeSlots = _serviceTimeRuleWriteRepository.GenerateTimeSlots(timeRule);
                _timeSlotWriteRepository.AddRange(timeSlots);
            }

            await _healthServiceWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return Unit.Value;
        }

    }
}
