using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Blogs;
using Hospital.Domain.Entities.Blogs;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Blogs
{
    public class AddBlogCommandHandler : BaseCommandHandler, IRequestHandler<AddBlogCommand, string>
    {
        private readonly IBlogWriteRepository _blogRepository;
        private readonly IMapper _mapper;
        public AddBlogCommandHandler(IStringLocalizer<Resources> localizer ,IBlogWriteRepository blogRepository, IMapper mapper)
            : base(localizer)
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
