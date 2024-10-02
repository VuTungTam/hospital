using MediatR;
using Hospital.Application.Dtos.Blogs;

namespace Hospital.Application.Queries.Blog.GetBlogs
{
    public class GetBlogQuery : IRequest<List<BlogDto>>
    {
    }
}
