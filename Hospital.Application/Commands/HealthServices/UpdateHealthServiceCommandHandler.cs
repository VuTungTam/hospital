using AutoMapper;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.ServiceTimeRules;
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
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var option = new QueryOption
            {
                Includes = new string[] { nameof(HealthService.ServiceTimeRules) }
            };

            var oldService = await _healthServiceReadRepository.GetByIdAsync(id, option, cancellationToken);

            if (oldService == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataDoesNotExistOrWasDeleted"]);
            }

            await _healthServiceWriteRepository.UpdateServiceAsync(oldService, request.HealthService, cancellationToken);

            return Unit.Value;
        }

    }
}
