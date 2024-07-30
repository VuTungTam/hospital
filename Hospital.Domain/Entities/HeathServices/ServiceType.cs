using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.HeathServices
{
    [Table("ServiceTypes")]
    public class ServiceType
       : BaseEntity,
        IModified,
        IModifier,
        ICreated,
        ICreator,
        ISoftDelete,
        IDeletedBy
    {
        public string Name { get; set; }
        public List<HealthService> Services { get; set; }
        public DateTime? Modified { get; set; }
        public long? Modifier { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public long? Creator { get; set; }
        public DateTime? Deleted { get; set; }
        public long? DeletedBy { get; set; }
    }
}
