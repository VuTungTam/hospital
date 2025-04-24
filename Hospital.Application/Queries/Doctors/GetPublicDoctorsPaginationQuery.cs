using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Models.Doctors;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Enums;

namespace Hospital.Application.Queries.Doctors
{
    public class GetPublicDoctorsPaginationQuery : BaseAllowAnonymousQuery<PaginationResult<PublicDoctorDto>>
    {
        public GetPublicDoctorsPaginationQuery(Pagination pagination, FilterDoctorRequest request, AccountStatus state)
        {
            Pagination = pagination;
            State = state;
            Request = request;
        }

        public Pagination Pagination { get; }
        public FilterDoctorRequest Request { get; }
        public AccountStatus State { get; }
    }
}
