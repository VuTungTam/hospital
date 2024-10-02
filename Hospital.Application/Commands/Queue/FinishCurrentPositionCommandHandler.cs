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
    public class FinishCurrentPositionCommandHandler : BaseCommandHandler, IRequestHandler<FinishCurrentPositionCommand>
    {
        private readonly IQueueItemReadRepository _queueItemReadRepository;
        private readonly IQueueItemWriteRepository _queueItemWriteRepository;
        public FinishCurrentPositionCommandHandler(
            IStringLocalizer<Resources> localizer,
            IQueueItemReadRepository queueItemReadRepository,
            IQueueItemWriteRepository queueItemWriteRepository
            ) : base(localizer)
        {
            _queueItemReadRepository = queueItemReadRepository;
            _queueItemWriteRepository = queueItemWriteRepository;
        }

        public async Task<Unit> Handle(FinishCurrentPositionCommand request, CancellationToken cancellationToken)
        {
            var curentQueueItem = await _queueItemReadRepository.GetCurrentAsync(request.ServiceId,cancellationToken: cancellationToken);
            if (curentQueueItem == null )
            {
                throw new BadRequestException("Thứ tự không hợp lệ");
            }
            else
            {
                curentQueueItem.State = 2;
                await _queueItemWriteRepository.UpdateAsync(curentQueueItem, cancellationToken:cancellationToken);
                var currentPosition = curentQueueItem.Position;
                QueueItem nextQueueItem = null;
                do
                {
                    currentPosition++;
                    nextQueueItem = await _queueItemReadRepository.GetByPositionAsync(request.ServiceId, currentPosition, curentQueueItem.Created.Date, cancellationToken: cancellationToken);
                }
                while (nextQueueItem != null && nextQueueItem.State == -1);
                if (nextQueueItem != null)
                {
                    nextQueueItem.State = 1;
                    await _queueItemWriteRepository.UpdateAsync(nextQueueItem, cancellationToken: cancellationToken);
                }
            }
            return Unit.Value;
        }
    }
}
