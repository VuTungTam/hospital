using MediatR;
using Hospital.Application.Dtos.Blogs;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Blogs
{
    public class UpdateBlogCommand : BaseCommand
    {
        public UpdateBlogCommand(BlogDto blog)
        {
            BlogDto = blog;
        }
        public BlogDto BlogDto { get; set; }
    }
}
