using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Base;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.SharedKernel.Application.CQRS.Queries
{
    public class GetAllQueryHandler<T, TResponse, TReadRepository> : BaseQueryHandler, IRequestHandler<GetAllQuery<T, TResponse>, List<TResponse>>
        where T : BaseEntity
        where TReadRepository : IReadRepository<T>
    {
        protected readonly IReadRepository<T> _readRepository;

        public GetAllQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            TReadRepository readRepository
        ) : base(authService, mapper, localizer)
        {
            _readRepository = readRepository;
        }

        public async Task<List<TResponse>> Handle(GetAllQuery<T, TResponse> request, CancellationToken cancellationToken)
        {
            var entities = await _readRepository.GetAsync(null,_readRepository.DefaultQueryOption,cancellationToken: cancellationToken);
            return _mapper.Map<List<TResponse>>(entities);
        }
    }

    public class GetAllQueryHandler<T, TResponse> : GetAllQueryHandler<T, TResponse, IReadRepository<T>> where T : BaseEntity
    {
        public GetAllQueryHandler(IAuthService authService, IMapper mapper, IStringLocalizer<Resources> localizer, IReadRepository<T> readRepository) : base(authService, mapper, localizer, readRepository)
        {
        }
    }
}
