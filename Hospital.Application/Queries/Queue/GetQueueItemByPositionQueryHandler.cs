using AutoMapper;
using Hospital.Application.Dtos.Queue;
using Hospital.Application.Repositories.Interfaces.Queue;
using Hospital.Domain.Entities.QueueItems;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Queue
{
    public class GetQueueItemByPositionQueryHandler : BaseQueryHandler, IRequestHandler<GetQueueItemByPositionQuery, QueueItemDto>
    {
        private readonly IQueueItemReadRepository _readQueueItemRepository;
        public GetQueueItemByPositionQueryHandler(
            IMapper mapper, IStringLocalizer<Resources> localizer,
            IQueueItemReadRepository readQueueItemRepository
            ) : base(mapper, localizer)
        {
            _readQueueItemRepository = readQueueItemRepository;
        }

        public async Task<QueueItemDto> Handle(GetQueueItemByPositionQuery request, CancellationToken cancellationToken)
        {
            var queueItem = await _readQueueItemRepository.GetByPositionAsync(request.Position, request.Created, cancellationToken);
            if (queueItem == null)
            {
                throw new BadRequestException("Số thứ tự không tồn tại");
            }
            else
            {
                var queueItemDto = _mapper.Map<QueueItemDto>(queueItem);
                return queueItemDto;
            }
        }
    }
}
