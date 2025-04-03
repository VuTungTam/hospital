using AutoMapper;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Specifications.ServiceTimeRules;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    public class AddServiceTimeRuleCommandHandler : BaseQueryHandler, IRequestHandler<AddServiceTimeRuleCommand, string>
    {
        private readonly IServiceTimeRuleWriteRepository _serviceTimeRuleWriteRepository;
        private readonly IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        private readonly ITimeSlotWriteRepository _timeSlotWriteRepository;
        public AddServiceTimeRuleCommandHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IServiceTimeRuleWriteRepository serviceTimeRuleWriteRepository,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository,
            ITimeSlotWriteRepository timeSlotWriteRepository
            ) : base(authService, mapper, localizer)
        {
            _serviceTimeRuleWriteRepository = serviceTimeRuleWriteRepository;
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _timeSlotWriteRepository = timeSlotWriteRepository;
        }

        public async Task<string> Handle(AddServiceTimeRuleCommand request, CancellationToken cancellationToken)
        {
            var timeRule = _mapper.Map<ServiceTimeRule>(request.TimeRule);

            var spec = new GetServiceTimeRuleByDayOfWeekSpecification(timeRule.DayOfWeek);

            spec.And(new GetServiceTimeRuleByServiceIdSpecification(timeRule.ServiceId));

            var existTimeRule = await _serviceTimeRuleReadRepository.GetAsync(spec, cancellationToken: cancellationToken);

            if (existTimeRule.Any())
            {
                throw new BadRequestException("Time rule đã được thêm");
            }

            _serviceTimeRuleWriteRepository.Add(timeRule);

            await _serviceTimeRuleWriteRepository.SaveChangesAsync(cancellationToken);

            var timeSlots = _serviceTimeRuleWriteRepository.GenerateTimeSlots(timeRule);

            _timeSlotWriteRepository.AddRange(timeSlots);

            await _serviceTimeRuleWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return timeRule.Id.ToString();
        }
    }
}
