using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Doctors
{
    public class DoctorByRankEqualsSpecification : ExpressionSpecification<Doctor>
    {
        public DoctorByRankEqualsSpecification(List<int> ranks) : base(x => ranks.Contains((int)x.DoctorRank))
        {
        }
    }
}
