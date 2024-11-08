using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Newes;
using Hospital.Domain.Entities.Newses;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Libraries.Helpers;
using Hospital.SharedKernel.Libraries.Utils;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Newes
{
    public class AddNewsCommandHandler : BaseCommandHandler, IRequestHandler<AddNewsCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly INewsWriteRepository _newsWriteRepository;

        public AddNewsCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            INewsWriteRepository newsWriteRepository
        ) : base(eventDispatcher, authService, localizer)
        {
            _mapper = mapper;
            _newsWriteRepository = newsWriteRepository;
        }

        public async Task<string> Handle(AddNewsCommand request, CancellationToken cancellationToken)
        {
            var news = _mapper.Map<News>(request.News);
            news.Slug = Utility.GenerateSlug(news.Title) + "-" + Utility.RandomNumber(8);

            if (news.Status == NewsStatus.Active && (news.PostDate == null || news.PostDate == default))
            {
                news.PostDate = DateTime.Now;
            }

            var t = StringHelper.GenerateTOC(news.Content, news.Slug);
            var t2 = StringHelper.GenerateTOC(news.ContentEn, news.Slug);

            news.Toc = t.Toc;
            news.Content = t.Html;
            news.TocEn = t2.Toc;
            news.ContentEn = t2.Html;

            await _newsWriteRepository.AddAsync(news, cancellationToken);

            return news.Id.ToString();
        }
    }
}
