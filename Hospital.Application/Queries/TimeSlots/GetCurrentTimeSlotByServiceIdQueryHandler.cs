using System.Runtime.InteropServices;
using AutoMapper;
using Hospital.Application.Dtos.TimeSlots;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Entities.TimeSlots;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.TimeSlots
{
    public class GetCurrentTimeSlotByServiceIdQueryHandler : BaseQueryHandler, IRequestHandler<GetCurrentTimeSlotByServiceIdQuery, TimeSlotDto>
    {
        private ITimeSlotReadRepository _timeSlotReadRepository;
        private IHealthServiceReadRepository _healthServiceReadRepository;
        private IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        public GetCurrentTimeSlotByServiceIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            ITimeSlotReadRepository timeSlotReadRepository,
            IStringLocalizer<Resources> localizer,
            IHealthServiceReadRepository healthServiceReadRepository,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository
            ) : base(authService, mapper, localizer)
        {
            _timeSlotReadRepository = timeSlotReadRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
        }

        public async Task<TimeSlotDto> Handle(GetCurrentTimeSlotByServiceIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);

            var option = new QueryOption
            {
                Includes = new[] { nameof(HealthService.ServiceTimeRules) }
            };

            var service = await _healthServiceReadRepository.GetByIdAsync(request.Id, option, cancellationToken);
            if (service == null)
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);

            var now = DateTime.Now;
            var todayDOW = (int)now.DayOfWeek;
            var timeNow = now.TimeOfDay;

            var timeRule = service.ServiceTimeRules
                .FirstOrDefault(tr => tr.DayOfWeek == todayDOW);

            if (timeRule == null)
                throw new BadRequestException(_localizer["CommonMessage.NoTimeRuleForToday"]);

            var timeSlots = await _timeSlotReadRepository.GetByTimeRuleIdAsync(timeRule.Id, cancellationToken);
            if (timeSlots == null || !timeSlots.Any())
                throw new BadRequestException(_localizer["CommonMessage.NoTimeSlotAvailable"]);

            var timeSlot = timeSlots.FirstOrDefault(ts => ts.Start <= timeNow && ts.End > timeNow);
            if (timeSlot == null)
                throw new BadRequestException(_localizer["CommonMessage.NoTimeSlotMatchingNow"]);

            return _mapper.Map<TimeSlotDto>(timeSlot);
        }

    }
}
