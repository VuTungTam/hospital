using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Symptoms;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Runtime.CompilerServices;

namespace Hospital.Application.Commands.Symptoms
{
    public class AddSymptomCommandHandler : BaseCommandHandler, IRequestHandler<AddSymptomCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly ISymptomWriteRepository _symptomWriteRepository;
        public AddSymptomCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            ISymptomWriteRepository symptomWriteRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _mapper = mapper;
            _symptomWriteRepository = symptomWriteRepository;
        }

        public async Task<string> Handle(AddSymptomCommand request, CancellationToken cancellationToken)
        {
            var symptom = _mapper.Map<Symptom>(request.Symptom);
            await _symptomWriteRepository.AddAsync(symptom, cancellationToken);
            return symptom.Id.ToString();
        }
    }
}
