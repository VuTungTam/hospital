using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Blogs
{
    public class BlogDto : BaseDto
    {
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string DescriptionVn { get; set; }
        public string DescriptionEn { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }

    }
    public class BLogValidator : BaseAbstractValidator<BlogDto>
    {
        public BLogValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.NameVn).NotEmpty().WithMessage(localizer["blog_name_vn_is_not_empty"]);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage(localizer["blog_name_en_is_not_empty"]);
            RuleFor(x => x.DescriptionVn).NotEmpty().WithMessage(localizer["blog_description_vn_is_not_empty"]);
            RuleFor(x => x.DescriptionEn).NotEmpty().WithMessage(localizer["blog_description_en_is_not_empty"]);
            RuleFor(x => x.Author).NotEmpty().WithMessage(localizer["blog_author_is_not_empty"]);
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage(localizer["blog_image_url_is_not_empty"]);
        }
    }
}
