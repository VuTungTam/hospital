using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthServices
{
    public class GetHealthServicesBySpecialtyIdSpecification : ExpressionSpecification<HealthService>
    {
        public GetHealthServicesBySpecialtyIdSpecification(long specialtyId) : base(x => (x.FacilitySpecialty.SpecialtyId == specialtyId))
        {
        }
    }
}
