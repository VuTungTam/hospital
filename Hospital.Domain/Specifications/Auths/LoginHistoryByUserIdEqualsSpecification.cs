using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Auths
{
    public class LoginHistoryByUserIdEqualsSpecification : ExpressionSpecification<LoginHistory>
    {
        public LoginHistoryByUserIdEqualsSpecification(long userId) : base(x => x.UserId == userId)
        {
        }
    }
}
