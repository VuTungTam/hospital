using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Blogs;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;

namespace Hospital.Application.Commands.Blogs
{
    public class DeleteBlogCommandHandler : BaseCommandHandler, IRequestHandler<DeleteBlogCommand>
    {
        private readonly IMapper _mapper;
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IBlogWriteRepository _blogWriteRepository;
        public DeleteBlogCommandHandler(IBlogWriteRepository blogWriteRepository, IBlogReadRepository blogReadRepository, IMapper mapper)
        {
            _blogReadRepository = blogReadRepository;
            _blogWriteRepository = blogWriteRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
            if(request.Ids == null || request.Ids.Exists(id => id <=0))
            {
                throw new BadRequestException("common_id_is_not_valid");
            }
            var blogs = await _blogReadRepository.GetByIdsAsync(request.Ids, cancellationToken:cancellationToken);
            if (blogs.Any())
            {
                await _blogWriteRepository.DeleteAsync(blogs, cancellationToken);
            }
            return Unit.Value;
            
        }
    }
}
