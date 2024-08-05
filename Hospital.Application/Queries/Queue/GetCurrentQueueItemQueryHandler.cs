using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Queue;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Queue
{
    public class GetCurrentQueueItemQueryHandler : BaseQueryHandler, IRequestHandler<GetCurrentQueueItemQuery, int>
    {
        private readonly IQueueItemReadRepository _queueItemReadRepository;
        public GetCurrentQueueItemQueryHandler(IMapper mapper, 
            IStringLocalizer<Resources> localizer,
            IQueueItemReadRepository queueItemReadRepository
            ) : base(mapper, localizer)
        {
            _queueItemReadRepository = queueItemReadRepository;
        }

        public async Task<int> Handle(GetCurrentQueueItemQuery request, CancellationToken cancellationToken)
        {
            var curentQueueItem =  await _queueItemReadRepository.GetCurrentAsync(cancellationToken: cancellationToken);
            if (curentQueueItem == null)
            {
                throw new BadRequestException("Thứ tự không hợp lệ");
            }
            return curentQueueItem.Position;
        }
    }
}
