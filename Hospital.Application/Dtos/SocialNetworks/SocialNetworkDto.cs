using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;
using FluentValidation;
using System.Globalization;
namespace Hospital.Application.Dtos.SocialNetworks
{
    public class SocialNetworkDto : BaseDto
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Logo { get; set; }
        
    }
    public class SocialNetworkDtoValidator : BaseAbstractValidator<SocialNetworkDto>
    {
        public SocialNetworkDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            var culture = CultureInfo.CurrentCulture;
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["social_network_name_is_not_empty"]);
            RuleFor(x => x.Link).NotEmpty().WithMessage(localizer["social_network_link_is_not_empty"]);
            RuleFor(x => x.Logo).NotEmpty().WithMessage(localizer["social_network_logo_is_not_empty"]);
        }
    }
}
