using Hospital.Application.Dtos.SocialNetworks;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.SocialNerworks
{
    public class GetAllSocialNetworkPagingQuery : BaseAllowAnonymousQuery<PagingResult<SocialNetworkDto>>
    {
        public GetAllSocialNetworkPagingQuery(Pagination pagination)
        {
            Pagination = pagination;
        }
        public Pagination Pagination { get; set; }
    }
}
