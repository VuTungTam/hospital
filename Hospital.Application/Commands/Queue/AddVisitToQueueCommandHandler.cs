using AutoMapper;
using Hospital.Application.Dtos.Queue;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.Queue;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Entities.QueueItems;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Specifications;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Queue
{
    public class AddBookingToQueueCommandHandler : BaseCommandHandler, IRequestHandler<AddBookingToQueueCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IQueueItemReadRepository _queueItemReadRepository;
        private readonly IQueueItemWriteRepository _queueItemWriteRepository;
        public AddBookingToQueueCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingReadRepository BookingReadRepository,
            IQueueItemReadRepository queueItemReadRepository,
            IQueueItemWriteRepository queueItemWriteRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _bookingReadRepository = BookingReadRepository;
            _queueItemWriteRepository = queueItemWriteRepository;
            _queueItemReadRepository = queueItemReadRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddBookingToQueueCommand request, CancellationToken cancellationToken)
        {
            var Booking = await _bookingReadRepository.GetByIdAsync(request.BookingId,_bookingReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
            if (Booking == null)
            {
                throw new BadRequestException("Lượt khám không tồn tại");
            }
            var serviceId = Booking.ServiceId;
            int lastPosition = await _queueItemReadRepository.GetQuantityTodayAsync(serviceId,cancellationToken);
            var queueItemDto = new QueueItemDto
            {
                BookingId = request.BookingId.ToString(),
                Position = lastPosition + 1,
                State = lastPosition == 0 ? 1 : 0
            };
            await ValidateAndThrowAsync(request.BookingId, cancellationToken);
            var queueItem = _mapper.Map<QueueItem>(queueItemDto);
            await _queueItemWriteRepository.AddAsync(queueItem, cancellationToken: cancellationToken);
            return queueItem.Position;
        }
        private async Task ValidateAndThrowAsync(long BookingId, CancellationToken cancellationToken)
        {
            if (BookingId > 0)
            {
                await InternalValidateAsync(new ExpressionSpecification<QueueItem>(x => x.BookingId == BookingId && x.Created.Date == DateTime.Now.Date), "Hồ sơ đã được xếp hàng");
            }

            async Task InternalValidateAsync(ExpressionSpecification<QueueItem> spec, string localizeKey)
            {
                var entity = await _queueItemReadRepository.FindBySpecificationAsync(spec,_bookingReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
                if (entity != null)
                {
                    throw new BadRequestException(_localizer[localizeKey]);
                }
            }
        }
    }
}
