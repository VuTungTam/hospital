using Hospital.Domain.Entities.HealthServices;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.HealthServices
{
    public class GetHealthServicesByDoctorIdSpecification : ExpressionSpecification<HealthService>
    {
        public GetHealthServicesByDoctorIdSpecification(long doctorId) : base(x => (x.DoctorId == doctorId))
        {
        }
    }
}
