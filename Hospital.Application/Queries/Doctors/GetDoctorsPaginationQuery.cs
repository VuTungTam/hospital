using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Models.Doctors;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Doctors
{
    [RequiredPermission(ActionExponent.ViewDoctor)]
    public class GetDoctorsPaginationQuery : BaseQuery<PaginationResult<DoctorDto>>
    {
        public GetDoctorsPaginationQuery(Pagination pagination, AccountStatus state, FilterDoctorRequest request)
        {
            Pagination = pagination;
            State = state;
            Request = request;
        }

        public Pagination Pagination { get; }
        public AccountStatus State { get; }
        public FilterDoctorRequest Request { get; }
    }
}
