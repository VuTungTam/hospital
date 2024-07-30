using Hospital.Application.Repositories.Interfaces.Declarations;
using Hospital.Application.Repositories.Interfaces.Queue;
using Hospital.Domain.Entities.QueueItems;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Queue
{
    public class AddDeclarationToQueueCommandHandler : BaseCommandHandler, IRequestHandler<AddDeclarationToQueueCommand, int>
    {
        private readonly IQueueItemReadRepository _queueItemReadRepository;
        private readonly IDeclarationReadRepository _declarationReadRepository;
        private readonly IQueueItemWriteRepository _queueItemWriteRepository;
        public AddDeclarationToQueueCommandHandler(
            IStringLocalizer<Resources> localizer,
            IQueueItemReadRepository queueItemReadRepository,
            IDeclarationReadRepository declarationReadRepository,
            IQueueItemWriteRepository queueItemWriteRepository
            ) : base(localizer)
        {
            _queueItemReadRepository = queueItemReadRepository;
            _queueItemWriteRepository = queueItemWriteRepository;
            _declarationReadRepository = declarationReadRepository;
        }

        public async Task<int> Handle(AddDeclarationToQueueCommand request, CancellationToken cancellationToken)
        {
            var declaration = await _declarationReadRepository.GetByIdAsync(request.DeclarationId, cancellationToken: cancellationToken);
            if (declaration == null)
            {
                throw new BadRequestException("Hồ sơ không tồn tại");
            }
            int lastPosition = await _queueItemReadRepository.GetQuantityTodayAsync(cancellationToken);
            var queueItem = new QueueItem {DeclarationId = request.DeclarationId, Date = DateTime.Now, Position = lastPosition+1, State = 0};
            await _queueItemWriteRepository.AddAsync(queueItem, cancellationToken: cancellationToken);
            return queueItem.Position;
        }
    }
}
