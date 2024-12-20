﻿using Hospital.Application.Repositories.Interfaces.Queue;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Queue
{
    public class RemovePositionCommandHandler : BaseCommandHandler, IRequestHandler<RemovePositionCommand>
    {
        private readonly IQueueItemReadRepository _queueItemReadRepository;
        private readonly IQueueItemWriteRepository _queueItemWriteRepository;
        public RemovePositionCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IQueueItemReadRepository queueItemReadRepository,
            IQueueItemWriteRepository queueItemWriteRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _queueItemReadRepository = queueItemReadRepository;
            _queueItemWriteRepository = queueItemWriteRepository;
        }

        public async Task<Unit> Handle(RemovePositionCommand request, CancellationToken cancellationToken)
        {
            var queueItem = await _queueItemReadRepository.GetByPositionAsync(request.ServiceId, request.Position, request.Created, cancellationToken);
            if( queueItem == null )
            {
                throw new BadRequestException("Số thứ tự không tồn tại");
            }
            else
            {
                queueItem.State = -1;
                await _queueItemWriteRepository.UpdateAsync(queueItem, cancellationToken: cancellationToken);
            }
            return Unit.Value;
        }
    }
}
