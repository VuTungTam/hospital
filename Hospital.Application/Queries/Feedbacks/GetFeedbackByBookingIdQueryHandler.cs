using AutoMapper;
using Hospital.Application.Dtos.Feedbacks;
using Hospital.Application.Repositories.Interfaces.Feedbacks;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Feedbacks
{
    public class GetFeedbackByBookingIdQueryHandler : BaseCommandHandler, IRequestHandler<GetFeedbackByBookingIdQuery, FeedbackDto>
    {
        private readonly IFeedbackReadRepository _feedbackReadRepository;
        public GetFeedbackByBookingIdQueryHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IFeedbackReadRepository feedbackReadRepository,
            IMapper mapper
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _feedbackReadRepository = feedbackReadRepository;
        }

        public async Task<FeedbackDto> Handle(GetFeedbackByBookingIdQuery request, CancellationToken cancellationToken)
        {
            var feedback = await _feedbackReadRepository.GetByBookingIdAsync(request.BookingId, cancellationToken: cancellationToken);
            if (feedback == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            return _mapper.Map<FeedbackDto>(feedback);
        }
    }
}
