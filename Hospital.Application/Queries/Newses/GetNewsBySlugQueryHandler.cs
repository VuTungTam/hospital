using AutoMapper;
using Hospital.Application.Dtos.Newes;
using Hospital.Application.Repositories.Interfaces.Newes;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Newses
{
    public class GetNewsBySlugQueryHandler : BaseQueryHandler, IRequestHandler<GetNewsBySlugQuery, NewsDto>
    {
        private readonly INewsReadRepository _newsReadRepository;

        public GetNewsBySlugQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            INewsReadRepository newsReadRepository
        ) : base(authService, mapper, localizer)
        {
            _newsReadRepository = newsReadRepository;
        }

        public async Task<NewsDto> Handle(GetNewsBySlugQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Slug))
            {
                return null;
            }

            var news = await _newsReadRepository.GetBySlugAsync(request.Slug, cancellationToken: cancellationToken);
            return _mapper.Map<NewsDto>(news);
        }
    }
}
