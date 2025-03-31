using Hospital.Domain.Entities.Doctors;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Doctors
{
    public class DoctorByStatusEqualsSpecification : ExpressionSpecification<Doctor>
    {
        public DoctorByStatusEqualsSpecification(AccountStatus status) : base(x => x.Status == status)
        {
        }
    }
}
