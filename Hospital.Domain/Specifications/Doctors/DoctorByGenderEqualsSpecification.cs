using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Doctors
{
    public class DoctorByGenderEqualsSpecification : ExpressionSpecification<Doctor>
    {
        public DoctorByGenderEqualsSpecification(List<int> genders) : base(x => genders.Contains((int)x.Gender))
        {
        }
    }
}
