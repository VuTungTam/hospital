using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications
{
    public class LimitByFacilityIdSpecification<T> : ExpressionSpecification<T> where T : BaseEntity
    {
        public LimitByFacilityIdSpecification(long facilityId) : base(x => (x as IFacility).FacilityId == facilityId)
        {
        }
    }

}
