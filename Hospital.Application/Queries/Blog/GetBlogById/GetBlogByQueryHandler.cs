using AutoMapper;
using Hospital.Application.Dtos.Blogs;
using Hospital.Application.Repositories.Interfaces.Blogs;
using MediatR;

namespace Hospital.Application.Queries.Blog.GetBlogById
{
    public class GetBlogByQueryHandler : IRequestHandler<GetBlogByIdQuery, BlogDto>
    {
        private IBlogReadRepository _blogReadRepository;
        private readonly IMapper _mapper;

        public GetBlogByQueryHandler(IBlogReadRepository blogReadRepository, IMapper mapper)
        {
            _blogReadRepository = blogReadRepository;
            _mapper = mapper;
        }
        public async Task<BlogDto> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
        {
            long id = request.BlogId;
            var blog = await _blogReadRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
            return _mapper.Map<BlogDto>(blog);
        }
    }
}
