using Hospital.Application.Dtos.DoctorWorkingContexts;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetDoctorContextQuery : BaseAllowAnonymousQuery<DoctorWorkingContextDto>
    {
        public GetDoctorContextQuery(long doctorId)
        {
            DoctorId = doctorId;
        }
        public long DoctorId { get; set; }
    }
}
