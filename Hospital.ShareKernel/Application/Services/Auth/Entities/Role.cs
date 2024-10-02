using Hospital.SharedKernel.Domain.Entities.Base;

namespace Hospital.SharedKernel.Application.Services.Auth.Entities
{
    public class Role : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public List<RoleAction> RoleActions { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }
}
