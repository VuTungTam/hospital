using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Doctors
{
    public class DoctorByTitleEqualsSpecification : ExpressionSpecification<Doctor>
    {
        public DoctorByTitleEqualsSpecification(DoctorTitle title) : base(x => x.DoctorTitle == title)
        {
        }
    }
}
