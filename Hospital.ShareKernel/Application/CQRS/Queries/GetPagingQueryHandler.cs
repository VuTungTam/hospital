using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Base;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.SharedKernel.Application.CQRS.Queries
{
    public class GetPagingQueryHandler<T, TResponse, TReadRepository> : BaseQueryHandler, IRequestHandler<GetPagingQuery<T, TResponse>, PaginationResult<TResponse>>
        where T : BaseEntity
        where TReadRepository : IReadRepository<T>
    {
        protected readonly IReadRepository<T> _readRepository;
        public GetPagingQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            TReadRepository readRepository
            ) : base(authService, mapper, localizer)
        {
            _readRepository = readRepository;
        }
        public async Task<PaginationResult<TResponse>> Handle(GetPagingQuery<T, TResponse> request, CancellationToken cancellationToken)
        {
            var result = await _readRepository.GetPagingAsync(request.Pagination, spec:null, _readRepository.DefaultQueryOption, cancellationToken: cancellationToken);
            return new PaginationResult<TResponse>(_mapper.Map<List<TResponse>>(result.Data), result.Total);
        }
    }
    public class GetPagingQueryHandler<T, TResponse> : GetPagingQueryHandler<T, TResponse, IReadRepository<T>> where T : BaseEntity
    {
        public GetPagingQueryHandler(IAuthService authService, IMapper mapper, IStringLocalizer<Resources> localizer, IReadRepository<T> readRepository) : base(authService, mapper, localizer, readRepository)
        {
        }
    }
}

