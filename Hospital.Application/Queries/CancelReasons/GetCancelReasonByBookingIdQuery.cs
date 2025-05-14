using Hospital.Application.Dtos.CancelReasons;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.CancelReasons
{
    public class GetCancelReasonByBookingIdQuery : BaseAllowAnonymousQuery<CancelReasonDto>
    {
        public GetCancelReasonByBookingIdQuery(long id)
        {
            Id = id;
        }
        public long Id { get; }
    }
}
