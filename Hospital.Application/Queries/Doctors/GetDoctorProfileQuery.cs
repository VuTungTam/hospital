using Hospital.Application.Dtos.Doctors;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Doctors
{
    public class GetDoctorProfileQuery : BaseQuery<DoctorDto>
    {
        public GetDoctorProfileQuery()
        {
            
        }
    }
}
