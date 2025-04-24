using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Doctors
{
    public class DoctorByDegreeEqualsSpecification : ExpressionSpecification<Doctor>
    {
        public DoctorByDegreeEqualsSpecification(List<int> degrees) : base(x => degrees.Contains((int)x.DoctorDegree))
        {
        }
    }
}
