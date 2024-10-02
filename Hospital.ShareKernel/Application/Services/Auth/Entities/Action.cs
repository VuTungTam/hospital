using Hospital.SharedKernel.Domain.Entities.Base;

namespace Hospital.SharedKernel.Application.Services.Auth.Entities
{
    public class Action :
        BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Exponent { get; set; }

        public bool IsInternal { get; set; }

        public virtual List<RoleAction> RoleActions { get; set; }
    }
}
