using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Blogs;
using Hospital.Domain.Entities.Blogs;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using MediatR;

namespace Hospital.Application.Commands.Blogs
{
    public class AddBlogCommandHandler : BaseCommandHandler, IRequestHandler<AddBlogCommand, string>
    {
        private readonly IBlogWriteRepository _blogRepository;
        private readonly IMapper _mapper;
        public AddBlogCommandHandler(IBlogWriteRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(AddBlogCommand request, CancellationToken cancellationToken)
        {
            var blog = _mapper.Map<Blog>(request.Blog);
            await _blogRepository.AddAsync(blog, cancellationToken);
            return blog.Id.ToString();
        }
    }
}
