using Hospital.Application.Dtos.Doctors;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Enums;

namespace Hospital.Application.Queries.Doctors
{
    public class GetDoctorsPaginationQuery : BaseAllowAnonymousQuery<PaginationResult<DoctorDto>>
    {
        public GetDoctorsPaginationQuery(Pagination pagination, List<long> specialtyIds, AccountStatus state, DoctorDegree degree, DoctorTitle title, DoctorRank rank)
        {
            Pagination = pagination;
            SpecialtyIds = specialtyIds;
            State = state;
            Degree = degree;
            Title = title;
            Rank = rank;
        }

        public Pagination Pagination { get; }
        public List<long> SpecialtyIds { get; }
        public AccountStatus State { get; }
        public DoctorDegree Degree { get; }
        public DoctorTitle Title { get; }
        public DoctorRank Rank { get; }
    }
}
