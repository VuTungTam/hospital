using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Events.BaseEvents;

namespace Hospital.Application.EventBus
{
    public class RegAccountDomainEvent : DomainEvent
    {
        public RegAccountDomainEvent(User user, string verificationCode, bool isUserFlow = true)
        {
            User = user;
            VerificationCode = verificationCode;
            IsUserFlow = isUserFlow;
        }

        public User User { get; }
        public string VerificationCode { get; }
        public bool IsUserFlow { get; }
    }
}
