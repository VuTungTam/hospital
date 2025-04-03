using AutoMapper;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Specifications.TimeSlots;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    public class DeleteServiceTimeRuleCommandHandler : BaseCommandHandler, IRequestHandler<DeleteServiceTimeRuleCommand>
    {
        private readonly IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        private readonly IServiceTimeRuleWriteRepository _serviceTimeRuleWriteRepository;
        private readonly ITimeSlotReadRepository _timeSlotReadRepository;
        private readonly ITimeSlotWriteRepository _timeSlotWriteRepository;
        public DeleteServiceTimeRuleCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository,
            IServiceTimeRuleWriteRepository serviceTimeRuleWriteRepository,
            ITimeSlotReadRepository timeSlotReadRepository,
            ITimeSlotWriteRepository timeSlotWriteRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _serviceTimeRuleWriteRepository = serviceTimeRuleWriteRepository;
            _timeSlotReadRepository = timeSlotReadRepository;
            _timeSlotWriteRepository = timeSlotWriteRepository;
        }

        public async Task<Unit> Handle(DeleteServiceTimeRuleCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || request.Ids.Exists(id => id <= 0))
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var rules = await _serviceTimeRuleReadRepository.GetByIdsAsync(request.Ids, cancellationToken: cancellationToken);
            if (rules.Any())
            {
                _serviceTimeRuleWriteRepository.Delete(rules);

                foreach (var rule in rules)
                {
                    var spec = new GetTimeSlotByTimeRuleIdSpecification(rule.Id);
                    var timeSlots = await _timeSlotReadRepository.GetAsync(spec, cancellationToken: cancellationToken);
                    if (timeSlots.Any())
                    {
                        _timeSlotWriteRepository.Delete(timeSlots);
                    }
                }
            }

            await _serviceTimeRuleWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            await _serviceTimeRuleWriteRepository.RemoveCacheWhenDeleteAsync(cancellationToken: cancellationToken);

            await _timeSlotWriteRepository.RemoveCacheWhenDeleteAsync(cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
