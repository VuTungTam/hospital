using FluentValidation;
using Hospital.Domain.Entities.Images;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Configures.Models;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.HealthServices
{
    public class ServiceTypeDto : BaseDto
    {
        public string Logo { get; set; }

        public string LogoUrl => CdnConfig.Get(Logo);

        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public string DescriptionVn { get; set; }

        public string DescriptionEn { get; set; }

        public string Image { get; set; }

        public string ImageUrl => CdnConfig.Get(Image);

        public string Slug { get; set; }
    }
    public class ServiceTypeValidator : BaseAbstractValidator<ServiceTypeDto>
    {
        public ServiceTypeValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.NameVn).NotEmpty().WithMessage(localizer["service_type_name_vn_is_not_empty"]);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage(localizer["service_type_name_en_is_not_empty"]);
        }
    }
}
