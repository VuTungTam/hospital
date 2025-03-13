using AutoMapper;
using Hospital.Application.Repositories.Interfaces.SocialNetworks;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.SocialNetworks
{
    public class AddSocialNetworkCommandHandler : BaseCommandHandler, IRequestHandler<AddSocialNetworkCommand, string>
    {
        private readonly ISocialNetworkReadRepository _socialNetworkReadRepository;
        private readonly ISocialNetworkWriteRepository _socialNetworkWriteRepository;
        private readonly IMapper _mapper;
        public AddSocialNetworkCommandHandler(
            IEventDispatcher eventDispatcher, 
            IAuthService authService, 
            IStringLocalizer<Resources> localizer,
            ISocialNetworkReadRepository socialNetworkReadRepository,
            ISocialNetworkWriteRepository socialNetworkWriteRepository,
            IMapper mapper
            ) : base(eventDispatcher, authService, localizer)
        {
            _socialNetworkReadRepository = socialNetworkReadRepository;
            _socialNetworkWriteRepository = socialNetworkWriteRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(AddSocialNetworkCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<SocialNetwork>(request.SocialNetworkDto);

            var existSocials = _socialNetworkReadRepository.GetAsync(null,_socialNetworkReadRepository.DefaultQueryOption,cancellationToken);

            //foreach (var existSocial in existSocials) {

            //}
            await _socialNetworkWriteRepository.AddAsync(entity,cancellationToken);

            return entity.Id.ToString();
        }
    }
}
