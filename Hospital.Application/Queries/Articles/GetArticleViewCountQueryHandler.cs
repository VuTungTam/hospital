using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Articles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Articles
{
    public class GetArticleViewCountQueryHandler : BaseQueryHandler, IRequestHandler<GetArticleViewCountQuery, int>
    {
        private readonly IArticleReadRepository _articleReadRepository;

        public GetArticleViewCountQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IArticleReadRepository articleReadRepository
        ) : base(authService, mapper, localizer)
        {
            _articleReadRepository = articleReadRepository;
        }

        public Task<int> Handle(GetArticleViewCountQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                return Task.FromResult(0);
            }
            return _articleReadRepository.GetViewCountAsync(request.Id, cancellationToken);
        }
    }
}
