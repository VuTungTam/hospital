using Hospital.Application.Dtos.SocialNetworks;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.SocialNetworks
{
    public class AddSocialNetworkCommand : BaseCommand<string>
    {
        public AddSocialNetworkCommand(SocialNetworkDto socialNetworkDto) 
        {
            SocialNetworkDto = socialNetworkDto;
        }
        public SocialNetworkDto SocialNetworkDto { get; set; }
    }
}
