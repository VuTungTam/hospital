using Hospital.Application.Dtos.Specialties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Specialties
{
    public class GetSpecialtyByIdQuery : BaseAllowAnonymousQuery<SpecialtyDto>
    {
        public GetSpecialtyByIdQuery(long id)
        {
            Id = id;
        }
        public long Id { get; }
    }
}
