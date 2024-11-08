using AutoMapper;
using Hospital.Application.Dtos.Newes;
using Hospital.Application.Repositories.Interfaces.Newes;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Newses
{
    public class GetNewsByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetNewsByIdQuery, NewsDto>
    {
        private readonly INewsReadRepository _newsReadRepository;

        public GetNewsByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            INewsReadRepository newsReadRepository
        ) : base(authService, mapper, localizer)
        {
            _newsReadRepository = newsReadRepository;
        }

        public async Task<NewsDto> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }
            var news = await _newsReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<NewsDto>(news);
        }
    }
}
