using Hospital.Domain.Entities.Specialties;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Specialties
{
    public class GetSpecialtiesByFacilityIdSpecification : ExpressionSpecification<Specialty>
    {
        public GetSpecialtiesByFacilityIdSpecification(long facilityId)
            : base(x => x.FacilitySpecialties.Any(fs => fs.FacilityId == facilityId))
        {
        }
    }

}
