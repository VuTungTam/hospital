using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications
{
    public class LimitByZoneIdSpecification<T> : ExpressionSpecification<T> where T : BaseEntity
    {
        public LimitByZoneIdSpecification(long zoneId) : base(x => (x as IZone).ZoneId == zoneId)
        {
        }
    }
}