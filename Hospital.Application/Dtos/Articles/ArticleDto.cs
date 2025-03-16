using FluentValidation;
using Hospital.Domain.Enums;
using Hospital.Domain.Models.Reactions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Libraries.Helpers;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Articles
{
    public class ArticleDto : BaseDto
    {
        public string Image { get; set; }

        public string Slug { get; set; }

        public string TitleSeo { get; set; }

        public string ImageUrl = ""; //=> CdnConfig.Get(Image);

        public string Title { get; set; }

        public string TitleEn { get; set; }

        public string Content { get; set; }

        public string ContentEn { get; set; }

        public string Summary { get; set; }

        public string SummaryEn { get; set; }

        public string Toc { get; set; }

        public string TocEn { get; set; }

        public DateTime? PostDate { get; set; }

        public ArticleStatus Status { get; set; }

        public string StatusText => Status.GetDescription();

        public bool IsHighlight { get; set; }

        public int ViewCount { get; set; }

        public int VirtualViewCount => (int)(long.Parse(Id) % 3000 + ViewCount);

        public ReactionModel Reaction { get; set; } = new();
    }

    public class ArticleDtoValidator : BaseAbstractValidator<ArticleDto>
    {
        public ArticleDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Image).Must(x => FileHelper.IsImageByFileName(x)).WithMessage(localizer["Utilities.ImageIsNotValid"]);
            RuleFor(x => x.Title).NotEmpty().WithMessage(localizer["Articles.TitleMustNotBeEmpty"]);
            RuleFor(x => x.TitleEn).NotEmpty().WithMessage(localizer["Articles.TitleEnglishMustNotBeEmpty"]);
            RuleFor(x => x.Content).NotEmpty().WithMessage(localizer["Articles.ContentMustNotBeEmpty"]);
            RuleFor(x => x.ContentEn).NotEmpty().WithMessage(localizer["Articles.ContentEnglishMustNotBeEmpty"]);
        }
    }
}
