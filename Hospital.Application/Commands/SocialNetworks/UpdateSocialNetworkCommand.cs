using Hospital.Application.Dtos.SocialNetworks;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using RabbitMQ.Client;

namespace Hospital.Application.Commands.SocialNetworks
{
    public class UpdateSocialNetworkCommand : BaseCommand
    {
        public UpdateSocialNetworkCommand(SocialNetworkDto socialNetworkDto) 
        {
            SocialNetworkDto = socialNetworkDto;
        }
        public SocialNetworkDto SocialNetworkDto { get; set; }
    }
}
