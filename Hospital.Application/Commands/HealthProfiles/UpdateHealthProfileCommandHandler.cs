using AutoMapper;
using Hospital.Application.Commands.HealthProfiles;
using Hospital.Application.Dtos.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Infrastructure.Services.Sms.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealhProfiles
{
    public class UpdateHealthProfileCommandHandler : BaseCommandHandler, IRequestHandler<UpdateHealthProfileCommand>
    {
        private readonly IHealthProfileReadRepository _healthProfileReadRepository;
        private readonly IHealthProfileWriteRepository _healthProfileWriteRepository;
        private readonly ILocationReadRepository _locationReadRepository;
        public UpdateHealthProfileCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            ILocationReadRepository locationReadRepository,
            IHealthProfileReadRepository healthProfileReadRepository,
            IHealthProfileWriteRepository healthProfileWriteRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _locationReadRepository = locationReadRepository;
            _healthProfileReadRepository = healthProfileReadRepository;
            _healthProfileWriteRepository = healthProfileWriteRepository;
        }

        public async Task<Unit> Handle(UpdateHealthProfileCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.HealthProfile.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var profile = await _healthProfileReadRepository.GetByIdAsync(id, _healthProfileReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);

            if (profile == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataDoesNotExistOrWasDeleted"]);
            }

            await ValidateAndThrowAsync(request.HealthProfile, cancellationToken);

            var entity = _mapper.Map<HealthProfile>(request.HealthProfile);

            entity.Pname = await _locationReadRepository.GetNameByIdAsync(entity.Pid, "province", cancellationToken); ;
            entity.Dname = await _locationReadRepository.GetNameByIdAsync(entity.Did, "district", cancellationToken);
            entity.Wname = await _locationReadRepository.GetNameByIdAsync(entity.Wid, "ward", cancellationToken);

            await _healthProfileWriteRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            return Unit.Value;
        }

        public async Task ValidateAndThrowAsync(HealthProfileDto profile, CancellationToken cancellationToken)
        {
            if (profile.Phone != null && profile.Phone != "")
            {
                if (!SmsUtility.IsVietnamesePhone(profile.Phone))
                {
                    throw new BadRequestException(_localizer["Account.PhoneIsNotValid"]);
                }
                else
                {
                    var phoneExist = await _healthProfileReadRepository.PhoneExistAsync(profile.Phone, exceptId: long.Parse(profile.Id), cancellationToken: cancellationToken);
                    if (phoneExist)
                    {
                        throw new BadRequestException(_localizer["Account.PhoneAlreadyExists"]);
                    }
                }
            }

            if (profile.Email != null && profile.Email != "")
            {
                if (!EmailUtility.IsEmail(profile.Email))
                {
                    throw new BadRequestException(_localizer["Account.EmailIsNotValid"]);
                }
                else
                {
                    var emailExist = await _healthProfileReadRepository.EmailExistAsync(profile.Email, exceptId: long.Parse(profile.Id), cancellationToken: cancellationToken);
                    if (emailExist)
                    {
                        throw new BadRequestException(_localizer["Account.EmailAlreadyExists"]);
                    }
                }
            }
        }
    }
}
