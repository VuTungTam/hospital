using AutoMapper;
using Hospital.Application.Dtos.CancelReasons;
using Hospital.Application.Repositories.Interfaces.CancelReasons;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.CancelReasons
{
    public class GetCancelReasonByBookingIdQueryHandler : BaseCommandHandler, IRequestHandler<GetCancelReasonByBookingIdQuery, CancelReasonDto>
    {
        private readonly ICancelReasonReadRepository _cancelReasonReadRepository;
        public GetCancelReasonByBookingIdQueryHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            ICancelReasonReadRepository cancelReasonReadRepository,
            IMapper mapper
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _cancelReasonReadRepository = cancelReasonReadRepository;
        }

        public async Task<CancelReasonDto> Handle(GetCancelReasonByBookingIdQuery request, CancellationToken cancellationToken)
        {
            var cancelReason = await _cancelReasonReadRepository.GetByBookingIdAsync(request.Id, cancellationToken: cancellationToken);

            if (cancelReason == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            return _mapper.Map<CancelReasonDto>(cancelReason);
        }
    }
}
