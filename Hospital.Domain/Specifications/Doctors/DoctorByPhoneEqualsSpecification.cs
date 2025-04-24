using Hospital.Domain.Entities.Doctors;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Doctors
{
    public class DoctorByPhoneEqualsSpecification : ExpressionSpecification<Doctor>
    {
        public DoctorByPhoneEqualsSpecification(string phone) : base(x => x.Phone == phone)
        {
        }
    }
}
