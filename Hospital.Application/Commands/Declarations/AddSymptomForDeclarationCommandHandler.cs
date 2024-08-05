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
    public class AddSymptomForDeclarationCommandHandler : BaseCommandHandler, IRequestHandler<AddSymptomForDeclarationCommand>
    {
        public readonly IDeclarationWriteRepository _declarationWriteRepository;
        public readonly IDeclarationReadRepository _declarationReadRepository;
        public readonly ISymptomReadRepository _symptomReadRepository;
        public AddSymptomForDeclarationCommandHandler(
            IStringLocalizer<Resources> localizer,
            IDeclarationWriteRepository declarationWriteRepository,
            IDeclarationReadRepository declarationReadRepository,
            ISymptomReadRepository symptomReadRepository
            ) : base(localizer)
        {
            _declarationReadRepository = declarationReadRepository;
            _declarationWriteRepository = declarationWriteRepository;
            _symptomReadRepository = symptomReadRepository;

        }

        public async Task<Unit> Handle(AddSymptomForDeclarationCommand request, CancellationToken cancellationToken)
        {
            if (!request.SymptomIds.Any())
            {
                throw new BadRequestException("Danh sách triệu chứng rỗng");
            }
            var declaration = await _declarationReadRepository.GetByIdAsync(request.DeclarationId, cancellationToken: cancellationToken);
            if (declaration == null)
            {
                throw new BadRequestException("Hồ sơ không tồn tại");
            }
            declaration.DeclarationSymptom ??= new();
            var symptoms = await _symptomReadRepository.GetByIdsAsync(request.SymptomIds, cancellationToken: cancellationToken);
            foreach (var symptom in symptoms)
            {
                if (symptom == null)
                {
                    throw new BadRequestException("Triệu chứng không tồn tại");
                }
                else
                {
                    declaration.DeclarationSymptom.Add(new DeclarationSymptom { DeclarationId = request.DeclarationId, SymptomId = symptom.Id });
                }
            }
            
            await _declarationWriteRepository.UpdateAsync(declaration, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
