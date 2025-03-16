using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.Specialties;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Specialties
{
    public class AddSpecialtyCommandHandler : BaseCommandHandler, IRequestHandler<AddSpecialtyCommand, string>
    {
        private readonly ISpecialtyWriteRepository _writeSpecialtyRepository;
        public AddSpecialtyCommandHandler(
            IEventDispatcher eventDispatcher, 
            IAuthService authService, 
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            ISpecialtyWriteRepository writeSpecialtyRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _writeSpecialtyRepository = writeSpecialtyRepository;
        }

        public async Task<string> Handle(AddSpecialtyCommand request, CancellationToken cancellationToken)
        {
            var specialty = _mapper.Map<Specialty>(request.Specialty);

            await _writeSpecialtyRepository.AddAsync(specialty, cancellationToken);

            return specialty.Id.ToString();
        }
    }
}
