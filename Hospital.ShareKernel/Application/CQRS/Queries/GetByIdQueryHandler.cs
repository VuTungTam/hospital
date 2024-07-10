using AutoMapper;
using MediatR;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Runtime.Exceptions;
using System.Resources;

namespace Hospital.SharedKernel.Application.CQRS.Queries
{
    public class GetByIdQueryHandler<T, TResponse, TReadRepository> : BaseQueryHandler, IRequestHandler<GetByIdQuery<T,TResponse>, TResponse> 
        where T : BaseEntity
        where TReadRepository : IReadRepository<T>
    {
        protected IReadRepository<T> _readRepository;
       
        public GetByIdQueryHandler(IMapper mapper, IReadRepository<T> readRepository):base(mapper)
        {
            _readRepository = readRepository;
        }

        public async Task<TResponse> Handle(GetByIdQuery<T, TResponse> request, CancellationToken cancellationToken)
        {
            if (!request.IsValidId())
            {
                throw new BadRequestException("common_id_is_not_valid");
            }
            else
            {
                var entity = await _readRepository.GetByIdAsync(request.GetId(), cancellationToken: cancellationToken);
                var dto = _mapper.Map<TResponse>(entity);
                return dto;
            }
        }

    }
    public class GetByIdQueryHandler<T, TResponse> : GetByIdQueryHandler<T, TResponse, IReadRepository<T>> where T : BaseEntity
    {
        public GetByIdQueryHandler(IMapper mapper, IReadRepository<T> readRepository) : base( mapper, readRepository)
        {
        }
    }
}
