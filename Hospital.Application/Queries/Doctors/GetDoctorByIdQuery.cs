using Hospital.Application.Dtos.Doctors;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Doctors
{

    public class GetDoctorByIdQuery : BaseQuery<DoctorDto>
    {
        public GetDoctorByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
