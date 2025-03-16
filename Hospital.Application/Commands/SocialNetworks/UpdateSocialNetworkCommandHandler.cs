using AutoMapper;
using Hospital.Application.Repositories.Interfaces.SocialNetworks;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.SocialNetworks
{
    public class UpdateSocialNetworkCommandHandler : BaseCommandHandler, IRequestHandler<UpdateSocialNetworkCommand>
    {
        private readonly ISocialNetworkReadRepository _socialNetworkReadRepository;
        private readonly ISocialNetworkWriteRepository _socialNetworkWriteRepository;
        public UpdateSocialNetworkCommandHandler(
            IEventDispatcher eventDispatcher, 
            IAuthService authService, 
            IStringLocalizer<Resources> localizer,
            ISocialNetworkReadRepository socialNetworkReadRepository,
            ISocialNetworkWriteRepository socialNetworkWriteRepository,
            IMapper mapper
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _socialNetworkReadRepository = socialNetworkReadRepository;
            _socialNetworkWriteRepository = socialNetworkWriteRepository;
        }

        public async Task<Unit> Handle(UpdateSocialNetworkCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.SocialNetworkDto.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var socialNetwork = await _socialNetworkReadRepository.GetByIdAsync(id,_socialNetworkReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
            if (socialNetwork == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            var entity = _mapper.Map<SocialNetwork>(request.SocialNetworkDto);

            await _socialNetworkWriteRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            return Unit.Value;


        }
    }
}
