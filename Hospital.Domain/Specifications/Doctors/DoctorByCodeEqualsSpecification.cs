using Hospital.Domain.Entities.Doctors;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Doctors
{
    public class DoctorByCodeEqualsSpecification : ExpressionSpecification<Doctor>
    {
        public DoctorByCodeEqualsSpecification(string code) : base(x => x.Code == code)
        {
        }
    }
}
