using Hospital.Application.Dtos.Newes;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Newses
{
    public class GetNewsByIdQuery : BaseAllowAnonymousQuery<NewsDto>
    {
        public GetNewsByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
