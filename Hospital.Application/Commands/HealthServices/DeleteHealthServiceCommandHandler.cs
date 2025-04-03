using AutoMapper;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealthServices
{
    public class DeleteHealthServiceCommandHandler : BaseCommandHandler, IRequestHandler<DeleteHealthServiceCommand>
    {
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly IHealthServiceWriteRepository _healthServiceWriteRepository;
        public DeleteHealthServiceCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IHealthServiceReadRepository healthServiceReadRepository,
            IHealthServiceWriteRepository healthServiceWriteRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _healthServiceReadRepository = healthServiceReadRepository;
            _healthServiceWriteRepository = healthServiceWriteRepository;
        }

        public async Task<Unit> Handle(DeleteHealthServiceCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || request.Ids.Exists(id => id <= 0))
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var symptoms = await _healthServiceReadRepository.GetByIdsAsync(request.Ids, _healthServiceReadRepository.DefaultQueryOption, cancellationToken);
            if (symptoms.Any())
            {
                await _healthServiceWriteRepository.DeleteAsync(symptoms, cancellationToken);
            }
            return Unit.Value;
        }
    }
}
