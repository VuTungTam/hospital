using Hospital.Application.Dtos.Specialties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Specialties
{
    public class GetSpecialtyByDoctorIdQuery : BaseAllowAnonymousQuery<List<SpecialtyDto>>
    {
        public GetSpecialtyByDoctorIdQuery(long doctorId)
        {
            DoctorId = doctorId;
        }
        public long DoctorId { get; }
    }
}
