using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Metas;
using Hospital.Domain.Entities.Metas;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Metas
{
    public class UpsertMetaTagCommandHandler : BaseCommandHandler, IRequestHandler<UpsertMetaTagCommand>
    {
        private readonly IMetaReadRepository _metaReadRepository;
        private readonly IMetaWriteRepository _metaWriteRepository;

        public UpsertMetaTagCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IMetaReadRepository metaReadRepository,
            IMetaWriteRepository metaWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _metaReadRepository = metaReadRepository;
            _metaWriteRepository = metaWriteRepository;
        }

        public async Task<Unit> Handle(UpsertMetaTagCommand request, CancellationToken cancellationToken)
        {
            var meta = _mapper.Map<Meta>(request.Meta);
            if (meta.Id <= 0)
            {
                //meta.AddDomainEvent(new AddMetaTagDomainEvent(meta));
                await _metaWriteRepository.AddAsync(meta, cancellationToken);
                return Unit.Value;
            }

            //meta.AddDomainEvent(new UpdateMetaTagDomainEvent(meta));
            await _metaWriteRepository.UpdateAsync(meta, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
