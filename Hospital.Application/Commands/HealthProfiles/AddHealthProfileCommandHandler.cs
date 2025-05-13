using AutoMapper;
using Hospital.Application.Dtos.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Constants;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Infrastructure.Services.Sms.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealthProfiles
{
    public class AddHealthProfileCommandHandler : BaseCommandHandler, IRequestHandler<AddHealthProfileCommand, string>
    {
        public readonly IHealthProfileWriteRepository _healthProfileWriteRepository;
        public readonly IHealthProfileReadRepository _healthProfileReadRepository;
        public readonly ILocationReadRepository _locationReadRepository;
        public readonly ISequenceRepository _sequenceRepository;
        public AddHealthProfileCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IHealthProfileWriteRepository healthProfileWriteRepository,
            IHealthProfileReadRepository healthProfileReadRepository,
            ISequenceRepository sequenceRepository,
            ILocationReadRepository locationReadRepository,
            IMapper mapper
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _healthProfileWriteRepository = healthProfileWriteRepository;
            _healthProfileReadRepository = healthProfileReadRepository;
            _locationReadRepository = locationReadRepository;
            _sequenceRepository = sequenceRepository;
        }

        public async Task<string> Handle(AddHealthProfileCommand request, CancellationToken cancellationToken)
        {
            await ValidateAndThrowAsync(request.Dto, cancellationToken);

            var healthProfile = _mapper.Map<HealthProfile>(request.Dto);

            await _healthProfileWriteRepository.AddProfileAsync(healthProfile, cancellationToken);

            await _healthProfileWriteRepository.RemoveCacheWhenAddAsync(healthProfile, cancellationToken);

            return healthProfile.Id.ToString();
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
                    var phoneExist = await _healthProfileReadRepository.PhoneExistAsync(profile.Phone, cancellationToken: cancellationToken);
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
                    var emailExist = await _healthProfileReadRepository.EmailExistAsync(profile.Email, cancellationToken: cancellationToken);
                    if (emailExist)
                    {
                        throw new BadRequestException(_localizer["Account.EmailAlreadyExists"]);
                    }
                }
            }
        }
    }
}
