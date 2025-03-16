using AutoMapper;
using Hospital.Application.Dtos.Articles;
using Hospital.Application.Repositories.Interfaces.Articles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Articles
{
    public class GetArticleByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetArticleByIdQuery, ArticleDto>
    {
        private readonly IArticleReadRepository _articleReadRepository;

        public GetArticleByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IArticleReadRepository articleReadRepository
        ) : base(authService, mapper, localizer)
        {
            _articleReadRepository = articleReadRepository;
        }

        public async Task<ArticleDto> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }
            var article = await _articleReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (article == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            return _mapper.Map<ArticleDto>(article);
        }
    }
}
