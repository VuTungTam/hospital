using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Blogs
{
    public class BlogDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }

    }
    public class BLogValidator : BaseAbstractValidator<BlogDto>
    {
        public BLogValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["blog_name_is_not_empty"]);
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizer["blog_description_is_not_empty"]);
            RuleFor(x => x.Author).NotEmpty().WithMessage(localizer["blog_author_is_not_empty"]);
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage(localizer["blog_image_url_is_not_empty"]);
        }
    }
}
