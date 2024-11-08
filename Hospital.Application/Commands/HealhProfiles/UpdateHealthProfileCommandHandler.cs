using AutoMapper;
using Hospital.Application.Dtos.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.Branches;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealhProfiles
{
    public class UpdateHealthProfileCommandHandler : BaseCommandHandler, IRequestHandler<UpdateHealthProfileCommand>
    {
        private readonly IMapper _mapper;
        private readonly IHealthProfileReadRepository _healthProfileReadRepository;
        private readonly IHealthProfileWriteRepository _healthProfileWriteRepository;
        public UpdateHealthProfileCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IHealthProfileReadRepository healthProfileReadRepository,
            IHealthProfileWriteRepository healthProfileWriteRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _mapper = mapper;
            _healthProfileReadRepository = healthProfileReadRepository;
            _healthProfileWriteRepository = healthProfileWriteRepository;
        }

        public async Task<Unit> Handle(UpdateHealthProfileCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.HealthProfile.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var entity = _mapper.Map<HealthProfile>(request.HealthProfile);

            var profile = _healthProfileReadRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

            if (profile == null) 
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            await _healthProfileWriteRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
