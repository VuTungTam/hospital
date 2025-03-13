using AutoMapper;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.ServiceTimeRules
{
    public class UpdateServiceTimeRuleCommandHandler : BaseCommandHandler, IRequestHandler<UpdateServiceTImeRuleCommand>
    {
        private readonly IMapper _mapper;
        private readonly IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        private readonly IServiceTimeRuleWriteRepository _serviceTimeRuleWriteRepository;
        public UpdateServiceTimeRuleCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService, 
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository,
            IServiceTimeRuleWriteRepository serviceTimeRuleWriteRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _mapper = mapper;
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
            _serviceTimeRuleWriteRepository = serviceTimeRuleWriteRepository;
        }

        public async Task<Unit> Handle(UpdateServiceTImeRuleCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.Dto.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var timeRule = await _serviceTimeRuleReadRepository.GetByIdAsync(id, _serviceTimeRuleReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
            if (timeRule == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }
            var entity = _mapper.Map<ServiceTimeRule>(request.Dto);

            await _serviceTimeRuleWriteRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            return Unit.Value;

        }
    }
}
