using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.ServiceTimeRules
{
    public class GetServiceTimeRuleByIdQuery : BaseAllowAnonymousQuery<ServiceTimeRuleDto>
    {
        public GetServiceTimeRuleByIdQuery(long id) 
        {
            Id = id;
        }

        public long Id { get;}
    }
}
