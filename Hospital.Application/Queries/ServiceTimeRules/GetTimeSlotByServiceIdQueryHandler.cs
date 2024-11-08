using AutoMapper;
using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.ServiceTimeRules
{
    public class GetTimeSlotByServiceIdQueryHandler : BaseQueryHandler, IRequestHandler<GetTimeSlotByServiceIdQuery, List<TimeFrame>>
    {
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        public GetTimeSlotByServiceIdQueryHandler(
            IAuthService authService, 
            IMapper mapper, 
            IStringLocalizer<Resources> localizer,
            IHealthServiceReadRepository healthServiceReadRepository,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthServiceReadRepository = healthServiceReadRepository;
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
        }

        public async Task<List<TimeFrame>> Handle(GetTimeSlotByServiceIdQuery request, CancellationToken cancellationToken)
        {
            var includes = new string[] { nameof(HealthService.TimeRules) };
            var service = await _healthServiceReadRepository.GetByIdAsync(request.ServiceId, includes, cancellationToken: cancellationToken);
            if (service == null)
            {
                throw new BadRequestException(_localizer["Khong co dich vu"]);
            }

            var timeRule = await _serviceTimeRuleReadRepository.GetByIdAsync(request.ServiceId, cancellationToken: cancellationToken);
            var dto = _mapper.Map<ServiceTimeRuleDto>(timeRule);
            return dto.TimeFrames;
        }
    }
}
