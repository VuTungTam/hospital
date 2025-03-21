using AutoMapper;
using Hospital.Application.Dtos.TimeSlots;
using Hospital.Application.Repositories.Interfaces.TimeSlots;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.TimeSlots
{
    public class GetTimeSlotByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetTimeSlotByIdQuery, TimeSlotDto>
    {
        private ITimeSlotReadRepository _timeSlotReadRepository;
        public GetTimeSlotByIdQueryHandler(
            IAuthService authService,
            IMapper mapper, 
            ITimeSlotReadRepository timeSlotReadRepository,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _timeSlotReadRepository = timeSlotReadRepository;
        }

        public async Task<TimeSlotDto> Handle(GetTimeSlotByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }
            var timeSlot = await _timeSlotReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (timeSlot == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            return _mapper.Map<TimeSlotDto>(timeSlot);
        }
    }
}
