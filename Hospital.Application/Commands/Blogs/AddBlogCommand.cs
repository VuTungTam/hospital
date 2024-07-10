using Hospital.Application.Dtos.Blogs;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Blogs
{
    public class AddBlogCommand : BaseCommand<string>
    {
        public AddBlogCommand(BlogDto blog)
        {
            Blog = blog;
        }
        public BlogDto Blog { get; set; }
    }
}
