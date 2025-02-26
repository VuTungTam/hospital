using AutoMapper;
using Hospital.Application.Dtos.SocialNetworks;
using Hospital.Application.Repositories.Interfaces.SocialNetworks;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.SocialNerworks
{
    public class GetAllSocialNetworkPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetAllSocialNetworkPagingQuery, PagingResult<SocialNetworkDto>>
    {
        private readonly ISocialNetworkReadRepository _socialNetworkReadRepository;
        public GetAllSocialNetworkPagingQueryHandler(
            IAuthService authService, 
            IMapper mapper, 
            IStringLocalizer<Resources> localizer,
            ISocialNetworkReadRepository socialNetworkReadRepository
            ) : base(authService, mapper, localizer)
        {
            _socialNetworkReadRepository = socialNetworkReadRepository;
        }

        public async Task<PagingResult<SocialNetworkDto>> Handle(GetAllSocialNetworkPagingQuery request, CancellationToken cancellationToken)
        {
            var paging = await _socialNetworkReadRepository.GetPagingAsync(request.Pagination, null, false, false, cancellationToken);
            
            var socialNetworks = _mapper.Map<List<SocialNetworkDto>>(paging.Data);

            return new PagingResult<SocialNetworkDto>(socialNetworks, paging.Total);
        }
    }
}
