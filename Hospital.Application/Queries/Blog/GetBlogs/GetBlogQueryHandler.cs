using AutoMapper;
using Hospital.Application.Dtos.Blogs;
using Hospital.Application.Repositories.Interfaces.Blogs;
using MediatR;

namespace Hospital.Application.Queries.Blog.GetBlogs
{
    public class GetBlogQueryHandler : IRequestHandler<GetBlogQuery, List<BlogDto>>
    {
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IMapper _mapper;

        public GetBlogQueryHandler(IBlogReadRepository blogReadRepository, IMapper mapper)
        {
            _blogReadRepository = blogReadRepository;
            _mapper = mapper;
        }
        public async Task<List<BlogDto>> Handle(GetBlogQuery request, CancellationToken cancellationToken)
        {
            var blogs = await _blogReadRepository.GetAsync(cancellationToken:cancellationToken);
            var blogList = _mapper.Map<List<BlogDto>>(blogs);
            return blogList;
        }
    }
}
