using Hospital.Application.Repositories.Interfaces.Declarations;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Application.Repositories.Interfaces.Visits;
using Hospital.Domain.Entities.Declarations;
using Hospital.Domain.Entities.Symptoms;
using Hospital.Domain.Entities.Visits;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Visits
{
    public class AddSymptomForVisitCommandHandler : BaseCommandHandler, IRequestHandler<AddSymptomForVisitCommand>
    {
        public readonly IVisitWriteRepository _visitWriteRepository;
        public readonly IVisitReadRepository _visitReadRepository;
        public readonly ISymptomReadRepository _symptomReadRepository;
        public AddSymptomForVisitCommandHandler(
            IStringLocalizer<Resources> localizer,
            IVisitWriteRepository visitWriteRepository,
            IVisitReadRepository visitReadRepository,
            ISymptomReadRepository symptomReadRepository
            ) : base(localizer)
        {
            _visitReadRepository = visitReadRepository;
            _visitWriteRepository = visitWriteRepository;
            _symptomReadRepository = symptomReadRepository;

        }

        public async Task<Unit> Handle(AddSymptomForVisitCommand request, CancellationToken cancellationToken)
        {
            if (!request.SymptomIds.Any())
            {
                throw new BadRequestException("Danh sách triệu chứng rỗng");
            }
            var visit = await _visitReadRepository.GetByIdAsync(request.VisitId, cancellationToken: cancellationToken);
            if (visit == null)
            {
                throw new BadRequestException("Lượt khám không tồn tại");
            }
            visit.VisitSymptom ??= new();
            var symptoms = await _symptomReadRepository.GetByIdsAsync(request.SymptomIds, cancellationToken: cancellationToken);
            foreach (var symptom in symptoms)
            {
                if (symptom == null)
                {
                    throw new BadRequestException("Triệu chứng không tồn tại");
                }
                else
                {
                    visit.VisitSymptom.Add(new VisitSymptom { VisitId = request.VisitId, SymptomId = symptom.Id });
                }
            }

            await _visitWriteRepository.UpdateAsync(visit, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
