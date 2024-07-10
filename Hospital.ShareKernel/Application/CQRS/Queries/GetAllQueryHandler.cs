using AutoMapper;
using MediatR;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Base;
using System.Resources;

namespace Hospital.SharedKernel.Application.CQRS.Queries
{
    public class GetAllQueryHandler<T, TResponse, TReadRepository> : BaseQueryHandler, IRequestHandler<GetAllQuery<T, TResponse>, List<TResponse>>
        where T : BaseEntity
        where TReadRepository : IReadRepository<T>
    {
        protected readonly IReadRepository<T> _readRepository;
        public GetAllQueryHandler(

        IMapper mapper,
            TReadRepository readRepository
        ) : base( mapper )
        { 
            _readRepository = readRepository;
        }

        public async Task<List<TResponse>> Handle(GetAllQuery<T, TResponse> request, CancellationToken cancellationToken)
        {
            var entities = await _readRepository.GetAsync(cancellationToken: cancellationToken);
            return _mapper.Map <List<TResponse>> (entities);
        }
    }
    public class GetAllQueryHandler<T, TResponse> : GetAllQueryHandler<T, TResponse, IReadRepository<T>> where T : BaseEntity
    {
        public GetAllQueryHandler(IMapper mapper, IReadRepository<T> readRepository) : base(mapper, readRepository)
        {
        }
    }
}
