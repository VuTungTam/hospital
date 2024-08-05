using Hospital.Domain.Entities.Declarations;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.QueueItems
{
    [Table("QueueItems")]
    public class QueueItem : BaseEntity, ICreated
    {
        public Declaration Declaration { get; set; }
        public long DeclarationId { get; set; }
        public int Position { get; set; }
        public int State { get; set; }
        public DateTime Created { get; set; }
    }
}
