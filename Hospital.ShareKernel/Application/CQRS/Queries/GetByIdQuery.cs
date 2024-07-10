using MediatR;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.SharedKernel.Application.CQRS.Queries
{
    public class GetByIdQuery<T,TResponse> : BaseQuery<TResponse>
    {
        public GetByIdQuery(string id)
        {
            Id = id;
        }
        public string Id {  get; set; }
        public virtual bool IsValidId() => long.TryParse(Id, out var parsedId) && parsedId > 0;

        public long GetId() => long.Parse(Id);
    }
}
