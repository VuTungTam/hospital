using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Symptoms
{
    public class DeleteSymptomCommandHandler : BaseCommandHandler, IRequestHandler<DeleteSymptomCommand>
    {
        private readonly ISymptomReadRepository _symptomReadRepository;
        private readonly ISymptomWriteRepository _symptomWriteRepository;
        public DeleteSymptomCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            ISymptomReadRepository symptomReadRepository,
            ISymptomWriteRepository symptomWriteRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _symptomReadRepository = symptomReadRepository;
            _symptomWriteRepository = symptomWriteRepository;
        }

        public async Task<Unit> Handle(DeleteSymptomCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || request.Ids.Exists(id => id <= 0))
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var symptoms = await _symptomReadRepository.GetByIdsAsync(request.Ids);
            if (symptoms.Any())
            {
                await _symptomWriteRepository.DeleteAsync(symptoms,cancellationToken);
            }
            return Unit.Value;
        }
    }
}
