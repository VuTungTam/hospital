using Hospital.Application.Dtos.SocialNetworks;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.SocialNetworks
{
    [RequiredPermission(ActionExponent.NetworksManagement)]
    public class AddSocialNetworkCommand : BaseCommand<string>
    {
        public AddSocialNetworkCommand(SocialNetworkDto socialNetworkDto) 
        {
            SocialNetworkDto = socialNetworkDto;
        }
        public SocialNetworkDto SocialNetworkDto { get; set; }
    }
}
