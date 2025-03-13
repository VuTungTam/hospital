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
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Newes
{
    public class UpdateNewsCommandHandler : BaseCommandHandler, IRequestHandler<UpdateNewsCommand>
    {
        private readonly INewsReadRepository _newsReadRepository;
        private readonly INewsWriteRepository _newsWriteRepository;
        private readonly IMapper _mapper;

        public UpdateNewsCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            INewsReadRepository newsReadRepository,
            INewsWriteRepository newsWriteRepository,
            IMapper mapper
        ) : base(eventDispatcher, authService, localizer)
        {
            _newsReadRepository = newsReadRepository;
            _newsWriteRepository = newsWriteRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.News.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var news = await _newsReadRepository.GetByIdAsync(id,_newsReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
            if (news == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            var entity = _mapper.Map<News>(request.News);
            if (!string.IsNullOrEmpty(entity.Slug))
            {
                var suffix = string.Join("", entity.Slug.TakeLast(8));
                entity.Slug = Utility.GenerateSlug(entity.Title) + "-" + suffix;
            }
            else
            {
                entity.Slug = Utility.GenerateSlug(entity.Title) + "-" + Utility.RandomNumber(8);
            }

            if (entity.Status == NewsStatus.Active && (entity.PostDate == null || entity.PostDate == default))
            {
                entity.PostDate = DateTime.Now;
            }

            var t = StringHelper.GenerateTOC(entity.Content, entity.Slug);
            var t2 = StringHelper.GenerateTOC(entity.ContentEn, entity.Slug);

            entity.Toc = t.Toc;
            entity.Content = t.Html;
            entity.TocEn = t2.Toc;
            entity.ContentEn = t2.Html;

            await _newsWriteRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            return Unit.Value;
        }


    }
}
