using AutoMapper;
using Hospital.Application.Dtos.Queue;
using Hospital.Application.Repositories.Interfaces.Declarations;
using Hospital.Application.Repositories.Interfaces.Queue;
using Hospital.Application.Repositories.Interfaces.Symptoms;
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
    public class AddVisitToQueueCommandHandler : BaseCommandHandler, IRequestHandler<AddVisitToQueueCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IVisitReadRepository _visitReadRepository;
        private readonly IQueueItemReadRepository _queueItemReadRepository;
        private readonly IQueueItemWriteRepository _queueItemWriteRepository;
        public AddVisitToQueueCommandHandler(
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IVisitReadRepository visitReadRepository,
            IQueueItemReadRepository queueItemReadRepository,
            IQueueItemWriteRepository queueItemWriteRepository
            ) : base(localizer)
        {
            _visitReadRepository = visitReadRepository;
            _queueItemWriteRepository = queueItemWriteRepository;
            _queueItemReadRepository = queueItemReadRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddVisitToQueueCommand request, CancellationToken cancellationToken)
        {
            var visit = await _visitReadRepository.GetByIdAsync(request.VisitId, cancellationToken: cancellationToken);
            if (visit == null)
            {
                throw new BadRequestException("Lượt khám không tồn tại");
            }
            var serviceId = visit.ServiceId;
            int lastPosition = await _queueItemReadRepository.GetQuantityTodayAsync(serviceId,cancellationToken);
            var queueItemDto = new QueueItemDto
            {
                VisitId = request.VisitId.ToString(),
                Position = lastPosition + 1,
                State = lastPosition == 0 ? 1 : 0
            };
            await ValidateAndThrowAsync(request.VisitId, cancellationToken);
            var queueItem = _mapper.Map<QueueItem>(queueItemDto);
            await _queueItemWriteRepository.AddAsync(queueItem, cancellationToken: cancellationToken);
            return queueItem.Position;
        }
        private async Task ValidateAndThrowAsync(long visitId, CancellationToken cancellationToken)
        {
            if (visitId > 0)
            {
                await InternalValidateAsync(new ExpressionSpecification<QueueItem>(x => x.VisitId == visitId && x.Created.Date == DateTime.Now.Date), "Hồ sơ đã được xếp hàng");
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
