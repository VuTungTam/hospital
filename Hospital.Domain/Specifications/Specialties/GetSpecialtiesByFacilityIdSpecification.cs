using Hospital.Domain.Entities.Specialties;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Specialties
{
    public class GetSpecialtiesByFacilityIdSpecification : ExpressionSpecification<Specialty>
    {
        public GetSpecialtiesByFacilityIdSpecification(long facilityId)
        : base(x => x.FacilitySpecialties
                    .Where(fs => fs.FacilityId == facilityId)
                    .Select(fs => fs.SpecialtyId)
                    .Contains(x.Id))
        {
        }
    }
}
