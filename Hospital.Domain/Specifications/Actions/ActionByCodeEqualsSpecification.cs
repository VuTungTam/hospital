using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Actions
{
    public class ActionByCodeEqualsSpecification : ExpressionSpecification<Hospital.SharedKernel.Domain.Entities.Auths.Action>
    {
        public ActionByCodeEqualsSpecification(string code) : base(x => x.Code == code)
        {
        }
    }
}
