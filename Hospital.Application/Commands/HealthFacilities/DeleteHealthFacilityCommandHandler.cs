using AutoMapper;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealthFacilities
{
    public class DeleteHealthFacilityCommandHandler : BaseCommandHandler, IRequestHandler<DeleteHealthFacilityCommand>
    {
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        private readonly IHealthFacilityWriteRepository _healthFacilityWriteRepository;
        public DeleteHealthFacilityCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IHealthFacilityReadRepository healthFacilityReadRepository,
            IHealthFacilityWriteRepository healthFacilityWriteRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _healthFacilityReadRepository = healthFacilityReadRepository;
            _healthFacilityWriteRepository = healthFacilityWriteRepository;
        }

        public async Task<Unit> Handle(DeleteHealthFacilityCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || request.Ids.Exists(id => id <= 0))
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var facilities = await _healthFacilityReadRepository.GetByIdsAsync(request.Ids, _healthFacilityReadRepository.DefaultQueryOption, cancellationToken);
            if (facilities.Any())
            {
                await _healthFacilityWriteRepository.DeleteAsync(facilities, cancellationToken);
            }
            return Unit.Value;
        }
    }
}
