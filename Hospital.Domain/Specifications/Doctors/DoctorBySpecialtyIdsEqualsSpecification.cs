using Hospital.Domain.Entities.Doctors;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Doctors
{
    public class DoctorBySpecialtyIdsEqualsSpecification : ExpressionSpecification<Doctor>
    {
        public DoctorBySpecialtyIdsEqualsSpecification(List<long> ids)
            : base(x => x.DoctorSpecialties.Any(ds => ids.Contains(ds.SpecialtyId)))
        {
        }
    }
}
