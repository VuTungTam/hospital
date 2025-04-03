using AutoMapper;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealthServices
{
    public class UpdateHealthServiceCommandHandler : BaseCommandHandler, IRequestHandler<UpdateHealthServiceCommand>
    {
        private readonly IHealthServiceWriteRepository _healthServiceWriteRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        public UpdateHealthServiceCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IHealthServiceWriteRepository healthServiceWriteRepository,
            IHealthServiceReadRepository healthServiceReadRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _healthServiceWriteRepository = healthServiceWriteRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
        }

        public async Task<Unit> Handle(UpdateHealthServiceCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.HealthService.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var service = _healthServiceReadRepository.GetByIdAsync(id, _healthServiceReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);

            if (service == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }
            var entity = _mapper.Map<HealthService>(request.HealthService);

            await _healthServiceWriteRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
