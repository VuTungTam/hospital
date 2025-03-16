using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Auths
{
    public class RefreshTokenByExpiryDateGreaterThanSpecification : ExpressionSpecification<RefreshToken>
    {
        public RefreshTokenByExpiryDateGreaterThanSpecification(DateTime date) : base(x => x.ExpiryDate > date)
        {
        }
    }
}
