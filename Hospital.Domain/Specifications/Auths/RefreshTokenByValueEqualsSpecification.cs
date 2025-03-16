using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Auths
{
    public class RefreshTokenByValueEqualsSpecification : ExpressionSpecification<RefreshToken>
    {
        public RefreshTokenByValueEqualsSpecification(string value) : base(x => x.RefreshTokenValue == value)
        {
        }
    }
}
