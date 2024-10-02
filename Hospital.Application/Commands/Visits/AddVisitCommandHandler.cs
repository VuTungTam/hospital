using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Declarations;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Application.Repositories.Interfaces.Visits;
using Hospital.Domain.Entities.Declarations;
using Hospital.Domain.Entities.Visits;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Visits
{
    public class AddVisitCommandHandler : BaseCommandHandler, IRequestHandler<AddVisitCommand, long>
    {
        private readonly IDeclarationReadRepository _declarationReadRepository;
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly IVisitWriteRepository _visitWriteRepository;
        private readonly IMapper _mapper;
        public AddVisitCommandHandler(
            IStringLocalizer<Resources> localizer,
            IDeclarationReadRepository declarationReadRepository,
            IHealthServiceReadRepository healthServiceReadRepository,
            IVisitWriteRepository visitWriteRepository,
            IMapper mapper
            ) : base(localizer)
        {
            _declarationReadRepository = declarationReadRepository;
            _visitWriteRepository = visitWriteRepository;
            _healthServiceReadRepository = healthServiceReadRepository;
            _mapper = mapper;
        }

        public async Task<long> Handle(AddVisitCommand request, CancellationToken cancellationToken)
        {
            var visit = _mapper.Map<Visit>(request.Dto);
            await _visitWriteRepository.AddAsync(visit, cancellationToken);
            long declarationId = visit.Id;
            return declarationId;
        }
    }
}
