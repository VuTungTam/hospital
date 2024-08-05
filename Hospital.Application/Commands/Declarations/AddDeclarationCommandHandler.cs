using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Declarations;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Declarations;
using Hospital.Domain.Entities.Symptoms;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Declarations
{
    public class AddDeclarationCommandHandler : BaseCommandHandler, IRequestHandler<AddDeclarationCommand, long>
    {
        private readonly IMapper _mapper;
        public readonly IDeclarationWriteRepository _declarationWriteRepository;
        public readonly ISymptomReadRepository _symptomReadRepository;
        public readonly ILocationReadRepository _locationReadRepository;
        public AddDeclarationCommandHandler(
            IStringLocalizer<Resources> localizer,
            IDeclarationWriteRepository declarationWriteRepository,
            ISymptomReadRepository symptomReadRepository,
            ILocationReadRepository locationReadRepository,
            IMapper mapper
            ) : base(localizer)
        {
            _declarationWriteRepository = declarationWriteRepository;
            _symptomReadRepository = symptomReadRepository;
            _mapper = mapper;
            _locationReadRepository = locationReadRepository;
        }

        public async Task<long> Handle(AddDeclarationCommand request, CancellationToken cancellationToken)
        {
            var declaration = _mapper.Map<Declaration>(request.Dto);
            declaration.Pname = await _locationReadRepository.GetNameByIdAsync(declaration.Pid, "province", cancellationToken);
            declaration.Dname = await _locationReadRepository.GetNameByIdAsync(declaration.Did, "district", cancellationToken);
            declaration.Wname = await _locationReadRepository.GetNameByIdAsync(declaration.Wid, "ward", cancellationToken);
            await _declarationWriteRepository.AddAsync(declaration, cancellationToken);
            long declarationId = declaration.Id;
            return declarationId;
        }
    }
}
