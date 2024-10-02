using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Base;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.SharedKernel.Application.CQRS.Queries
{
    public class GetPagingQueryHandler<T, TResponse, TReadRepository> : BaseQueryHandler, IRequestHandler<GetPagingQuery<T, TResponse>, PagingResult<TResponse>>
        where T : BaseEntity
        where TReadRepository : IReadRepository<T>
    {
        protected readonly IReadRepository<T> _readRepository;

        public GetPagingQueryHandler(
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            TReadRepository readRepository
        ) : base(mapper, localizer)
        {
            _readRepository = readRepository;
        }

        public async Task<PagingResult<TResponse>> Handle(GetPagingQuery<T, TResponse> request, CancellationToken cancellationToken)
        {
            var result = await _readRepository.GetPagingAsync(request.Pagination, cancellationToken: cancellationToken);
            return new PagingResult<TResponse>(_mapper.Map<List<TResponse>>(result.Data), result.Total);
        }
    }
    public class GetPagingQueryHandler<T, TResponse> : GetPagingQueryHandler<T, TResponse, IReadRepository<T>> where T : BaseEntity
    {
        public GetPagingQueryHandler(IMapper mapper, IStringLocalizer<Resources> localizer, IReadRepository<T> readRepository) : base(mapper, localizer, readRepository)
        {
        }
    }
}

