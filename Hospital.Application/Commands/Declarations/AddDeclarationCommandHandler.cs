using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Declarations;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Declarations;
using Hospital.Domain.Entities.Symptoms;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
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
        public AddDeclarationCommandHandler(
            IStringLocalizer<Resources> localizer,
            IDeclarationWriteRepository declarationWriteRepository,
            ISymptomReadRepository symptomReadRepository,
            IMapper mapper
            ) : base(localizer)
        {
            _declarationWriteRepository = declarationWriteRepository;
            _symptomReadRepository = symptomReadRepository;
            _mapper = mapper;
        }

        public async Task<long> Handle(AddDeclarationCommand request, CancellationToken cancellationToken)
        {
            if ( !request.SymptomIds.Any()) {
                throw new BadRequestException("Danh sách triệu chứng rỗng");
            }
            var declaration = _mapper.Map<Declaration>(request.Dto);
            await _declarationWriteRepository.AddAsync(declaration, cancellationToken);
            long declarationId = declaration.Id;
            declaration.DeclarationSymptom ??= new();
            var symptoms = await _symptomReadRepository.GetByIdsAsync(request.SymptomIds, cancellationToken: cancellationToken);
            foreach(var symptom in symptoms)
            {
                if(symptom == null)
                {
                    throw new BadRequestException("Triệu chứng không tồn tại");
                }
                else
                {
                    declaration.DeclarationSymptom.Add(new DeclarationSymptom { DeclarationId = declarationId, SymptomId = symptom.Id });
                }
            }
            if (declaration == null)
            {
                throw new BadRequestException("Null");
            }
            await _declarationWriteRepository.UpdateAsync(declaration,cancellationToken: cancellationToken);
            return declarationId;
            
        }
    }
}
