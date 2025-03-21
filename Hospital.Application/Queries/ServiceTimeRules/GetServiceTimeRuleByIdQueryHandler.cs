using AutoMapper;
using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.ServiceTimeRules
{
    public class GetServiceTimeRuleByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetServiceTimeRuleByIdQuery, ServiceTimeRuleDto>
    {
        private readonly IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        public GetServiceTimeRuleByIdQueryHandler(
            IAuthService authService,
            IMapper mapper, 
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
        }

        public async Task<ServiceTimeRuleDto> Handle(GetServiceTimeRuleByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }
            var timeRule = await _serviceTimeRuleReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (timeRule == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            return _mapper.Map<ServiceTimeRuleDto>(timeRule);
        }
    }
}
