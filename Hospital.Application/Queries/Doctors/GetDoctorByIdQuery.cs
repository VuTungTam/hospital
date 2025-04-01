using Hospital.Application.Dtos.Doctors;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Doctors
{
    [RequiredPermission(ActionExponent.ViewDoctor)]
    public class GetDoctorByIdQuery : BaseQuery<DoctorDto>
    {
        public GetDoctorByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
