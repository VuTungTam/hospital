using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Users;

namespace Hospital.SharedKernel.Application.Services.Auth.Entities
{
    public class UserRole :
        BaseEntity,
        ICreated,
        ICreator,
        IModified,
        IModifier,
        ISoftDelete
    {
        public long RoleId { get; set; }

        public long UserId { get; set; }

        public Role Role { get; set; }

        public User User { get; set; }

        public DateTime Created { get; set; }

        public long? Creator { get; set; }

        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }
    }
}
