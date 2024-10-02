using MediatR;
using Hospital.Application.Dtos.Blogs;

namespace Hospital.Application.Queries.Blog.GetBlogById
{
    public class GetBlogByIdQuery : IRequest<BlogDto>
    {
        public GetBlogByIdQuery(long id)
        {
            BlogId = id;
        }
        public long BlogId { get; set; }
    }
}
