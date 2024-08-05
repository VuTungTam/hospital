using AutoMapper;
using Hospital.Application.Dtos.Queue;
using Hospital.Application.Repositories.Interfaces.Declarations;
using Hospital.Application.Repositories.Interfaces.Queue;
using Hospital.Domain.Entities.Declarations;
using Hospital.Domain.Entities.QueueItems;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Specifications;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Queue
{
    public class AddDeclarationToQueueCommandHandler : BaseCommandHandler, IRequestHandler<AddDeclarationToQueueCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IQueueItemReadRepository _queueItemReadRepository;
        private readonly IDeclarationReadRepository _declarationReadRepository;
        private readonly IQueueItemWriteRepository _queueItemWriteRepository;
        public AddDeclarationToQueueCommandHandler(
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IQueueItemReadRepository queueItemReadRepository,
            IDeclarationReadRepository declarationReadRepository,
            IQueueItemWriteRepository queueItemWriteRepository
            ) : base(localizer)
        {
            _queueItemReadRepository = queueItemReadRepository;
            _queueItemWriteRepository = queueItemWriteRepository;
            _declarationReadRepository = declarationReadRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddDeclarationToQueueCommand request, CancellationToken cancellationToken)
        {
            var declaration = await _declarationReadRepository.GetByIdAsync(request.DeclarationId, cancellationToken: cancellationToken);
            if (declaration == null)
            {
                throw new BadRequestException("Hồ sơ không tồn tại");
            }
            int lastPosition = await _queueItemReadRepository.GetQuantityTodayAsync(cancellationToken);
            var queueItemDto = new QueueItemDto
            {
                DeclarationId = request.DeclarationId.ToString(),
                Position = lastPosition + 1,
                State = lastPosition == 0 ? 1 : 0
            };
            await ValidateAndThrowAsync(request.DeclarationId, cancellationToken);
            var queueItem = _mapper.Map<QueueItem>(queueItemDto);
            await _queueItemWriteRepository.AddAsync(queueItem, cancellationToken: cancellationToken);
            return queueItem.Position;
        }
        private async Task ValidateAndThrowAsync(long declarationId, CancellationToken cancellationToken)
        {
            if (declarationId > 0)
            {
                await InternalValidateAsync(new ExpressionSpecification<QueueItem>(x => x.DeclarationId == declarationId && x.Created.Date == DateTime.Now.Date), "Hồ sơ đã được xếp hàng");
            }

            async Task InternalValidateAsync(ExpressionSpecification<QueueItem> spec, string localizeKey)
            {
                var entity = await _queueItemReadRepository.FindBySpecificationAsync(spec, cancellationToken: cancellationToken);
                if (entity != null)
                {
                    throw new BadRequestException(_localizer[localizeKey]);
                }
            }
        }
    }
}
