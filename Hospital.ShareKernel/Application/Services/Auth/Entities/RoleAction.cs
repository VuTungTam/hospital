using Hospital.SharedKernel.Domain.Entities.Base;

namespace Hospital.SharedKernel.Application.Services.Auth.Entities
{
    public class RoleAction : BaseEntity
    {
        public long RoleId { get; set; }

        public long ActionId { get; set; }

        public Role Role { get; set; }

        public Action Action { get; set; }
    }
}
