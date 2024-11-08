using FluentValidation;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Configures.Models;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Libraries.Helpers;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Newes
{
    public class NewsDto : BaseDto
    {
        public string Image { get; set; }

        public string Slug { get; set; }

        public string TitleSeo { get; set; }

        public string ImageUrl => CdnConfig.Get(Image);

        public string Title { get; set; }

        public string TitleEn { get; set; }

        public string Content { get; set; }

        public string ContentEn { get; set; }

        public string Summary { get; set; }

        public string SummaryEn { get; set; }

        public string Toc { get; set; }

        public string TocEn { get; set; }

        public DateTime? PostDate { get; set; }

        public NewsStatus Status { get; set; }

        public string StatusText => Status.GetDescription();

        public bool IsHighlight { get; set; }
    }

    public class NewsDtoValidator : BaseAbstractValidator<NewsDto>
    {
        public NewsDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Image).Must(x => FileHelper.IsImageByFileName(x)).WithMessage(localizer["utils_image_is_not_valid"]);
            RuleFor(x => x.Title).NotEmpty().WithMessage(localizer["news_title_must_not_be_empty"]);
            RuleFor(x => x.TitleEn).NotEmpty().WithMessage(localizer["news_title_en_must_not_be_empty"]);
            RuleFor(x => x.Content).NotEmpty().WithMessage(localizer["news_content_must_not_be_empty"]);
            RuleFor(x => x.ContentEn).NotEmpty().WithMessage(localizer["news_content_en_must_not_be_empty"]);
        }
    }
}
