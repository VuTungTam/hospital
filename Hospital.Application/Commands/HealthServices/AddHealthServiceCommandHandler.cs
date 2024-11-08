using AutoMapper;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealthServices
{
    public class AddHealthServiceCommandHandler : BaseCommandHandler, IRequestHandler<AddHealthServiceCommand, string>
    {

        private readonly IHealthServiceWriteRepository _healthServiceWriteRepository;
        private readonly IMapper _mapper;
        public AddHealthServiceCommandHandler(
            IEventDispatcher eventDispatcher, 
            IAuthService authService, 
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IHealthServiceWriteRepository healthServiceWriteRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _healthServiceWriteRepository = healthServiceWriteRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(AddHealthServiceCommand request, CancellationToken cancellationToken)
        {
            var service = _mapper.Map<HealthService>(request.HealthService);
            await _healthServiceWriteRepository.AddAsync(service, cancellationToken);
            return service.Id.ToString ();
        }
    }
}
