using Hospital.Domain.Entities.Doctors;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Doctors
{
    public class DoctorByEmailEqualsSpecification : ExpressionSpecification<Doctor>
    {
        public DoctorByEmailEqualsSpecification(string email) : base(x => x.Email == email)
        {
        }
    }
}
