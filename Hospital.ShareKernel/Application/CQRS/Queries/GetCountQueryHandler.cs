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
    public class GetCountQueryHandler<T, TReadRepository> : BaseQueryHandler, IRequestHandler<GetCountQuery<T>, int> where T : BaseEntity
    {
        protected readonly IReadRepository<T> _readRepository;

        public GetCountQueryHandler(
            IAuthService authService, 
            IMapper mapper, 
            IStringLocalizer<Resources> localizer,
            IReadRepository<T> readRepository
            ) : base(authService, mapper, localizer)
        {
            _readRepository = readRepository;
        }

        
        public Task<int> Handle(GetCountQuery<T> request, CancellationToken cancellationToken)
        {
            var rs = _readRepository.GetCountAsync();
            return rs;
        }
    }
    public class GetCountQueryHandler<T> : GetCountQueryHandler<T, IReadRepository<T>> where T : BaseEntity
    {
        public GetCountQueryHandler(IAuthService authService, IMapper mapper, IStringLocalizer<Resources> localizer, IReadRepository<T> readRepository) : base(authService, mapper, localizer, readRepository)
        {
        }
    }
}
