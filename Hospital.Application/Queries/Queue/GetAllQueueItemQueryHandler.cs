using AutoMapper;
using Hospital.Application.Dtos.Queue;
using Hospital.Application.Repositories.Interfaces.Queue;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Queue
{
    public class GetAllQueueItemQueryHandler : BaseQueryHandler, IRequestHandler<GetAllQueueItemQuery, List<QueueItemDto>>
    {
        private readonly IQueueItemReadRepository _queueItemReadRepository;
        public GetAllQueueItemQueryHandler(
            IMapper mapper, IStringLocalizer<Resources> localizer,
            IQueueItemReadRepository queueItemReadRepository
            ) : base(mapper, localizer)
        {
            _queueItemReadRepository = queueItemReadRepository;
        }

        public async Task<List<QueueItemDto>> Handle(GetAllQueueItemQuery request, CancellationToken cancellationToken)
        {
            var list = await _queueItemReadRepository.GetByDateAsync(request.Created, cancellationToken);
            if (list == null)
            {
                throw new BadRequestException("Không lấy được danh sách xếp hàng");
            }
            else
            {
                var listDto = _mapper.Map<List<QueueItemDto>>(list);
                return listDto;
            }
        }
    }
}
