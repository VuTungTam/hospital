using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Doctors
{
    public class DoctorByRankEqualsSpecification : ExpressionSpecification<Doctor>
    {
        public DoctorByRankEqualsSpecification(DoctorRank rank) : base(x => x.DoctorRank == rank)
        {
        }
    }
}
