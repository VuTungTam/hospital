using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Specialties
{
    public class DeleteSpecialtyCommandHandler : BaseCommandHandler, IRequestHandler<DeleteSpecialtyCommand>
    {
        private readonly ISpecialtyWriteRepository _specialtyWriteRepository;
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        public DeleteSpecialtyCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            ISpecialtyWriteRepository specialtyWriteRepository,
            ISpecialtyReadRepository specialtyReadRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _specialtyWriteRepository = specialtyWriteRepository;
            _specialtyReadRepository = specialtyReadRepository;
        }

        public async Task<Unit> Handle(DeleteSpecialtyCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || request.Ids.Exists(id => id <= 0))
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var symptoms = await _specialtyReadRepository.GetByIdsAsync(request.Ids, _specialtyReadRepository.DefaultQueryOption,cancellationToken);
            if (symptoms.Any())
            {
                await _specialtyWriteRepository.DeleteAsync(symptoms, cancellationToken);
            }
            return Unit.Value;
        }
    }
}
