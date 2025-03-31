using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications
{
    public class LimitByDoctorIdSpecification<T> : ExpressionSpecification<T> where T : BaseEntity
    {
        public LimitByDoctorIdSpecification(long zoneId) : base(x => (x as IDoctor).DoctorId == zoneId)
        {
        }
    }
}
