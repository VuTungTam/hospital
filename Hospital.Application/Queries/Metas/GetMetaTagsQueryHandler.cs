using AutoMapper;
using Hospital.Application.Dtos.Metas;
using Hospital.Application.Repositories.Interfaces.Metas;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Metas
{
    public class GetMetaTagsQueryHandler : BaseQueryHandler, IRequestHandler<GetMetaTagsQuery, List<MetaDto>>
    {
        private readonly IMetaReadRepository _metaReadRepository;

        public GetMetaTagsQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IMetaReadRepository metaReadRepository
        ) : base(authService, mapper, localizer)
        {
            _metaReadRepository = metaReadRepository;
        }

        public async Task<List<MetaDto>> Handle(GetMetaTagsQuery request, CancellationToken cancellationToken)
        {
            var metas = await _metaReadRepository.GetAsync(cancellationToken: cancellationToken);
            metas = metas.OrderBy(m => m.Id).ToList();

            return _mapper.Map<List<MetaDto>>(metas);
        }
    }
}
