﻿using AutoMapper;
using Hospital.Application.Commands.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealhProfiles
{
    public class DeleteHealthProfileCommandHandler : BaseCommandHandler, IRequestHandler<DeleteHealthProfileCommand>
    {
        private readonly IHealthProfileReadRepository _healthProfileReadRepository;
        private readonly IHealthProfileWriteRepository _healthProfileWriteRepository;
        public DeleteHealthProfileCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IHealthProfileReadRepository healthProfileReadRepository,
            IHealthProfileWriteRepository healthProfileWriteRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _healthProfileReadRepository = healthProfileReadRepository;
            _healthProfileWriteRepository = healthProfileWriteRepository;
        }

        public async Task<Unit> Handle(DeleteHealthProfileCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || request.Ids.Exists(id => id <= 0))
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var healthProfiles = await _healthProfileReadRepository.GetByIdsAsync(request.Ids, _healthProfileReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
            if (healthProfiles.Any())
            {
                await _healthProfileWriteRepository.DeleteAsync(healthProfiles, cancellationToken);

            }
            return Unit.Value;
        }
    }
}
