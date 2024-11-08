using AutoMapper;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    public class AddServiceTimeRuleCommandHandler : BaseQueryHandler, IRequestHandler<AddServiceTimeRuleCommand, string>
    {
        private readonly IServiceTimeRuleWriteRepository _serviceTimeRuleWriteRepository;
        public AddServiceTimeRuleCommandHandler(
            IAuthService authService, 
            IMapper mapper, 
            IStringLocalizer<Resources> localizer,
            IServiceTimeRuleWriteRepository serviceTimeRuleWriteRepository
            ) : base(authService, mapper, localizer)
        {
            _serviceTimeRuleWriteRepository = serviceTimeRuleWriteRepository;
        }

        public async Task<string> Handle(AddServiceTimeRuleCommand request, CancellationToken cancellationToken)
        {
            var timeRule = _mapper.Map<ServiceTimeRule>(request.TimeRule);

            await _serviceTimeRuleWriteRepository.AddAsync(timeRule, cancellationToken);

            return timeRule.Id.ToString();
        }
    }
}
