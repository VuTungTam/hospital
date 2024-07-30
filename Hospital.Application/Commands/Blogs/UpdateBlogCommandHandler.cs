using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Blogs;
using Hospital.Domain.Entities.Blogs;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Blogs
{
    public class UpdateBlogCommandHandler : BaseCommandHandler , IRequestHandler<UpdateBlogCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IBlogWriteRepository _blogWriteRepository;
        public UpdateBlogCommandHandler(IStringLocalizer<Resources> localizer, IBlogWriteRepository blogWriteRepository, IMapper mapper)
            : base(localizer)
        {
            _blogWriteRepository = blogWriteRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {
            var blog = _mapper.Map<Blog>(request.BlogDto);
            await _blogWriteRepository.UpdateAsync(blog, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
