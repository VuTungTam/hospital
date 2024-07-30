using Hospital.SharedKernel.Domain.Entities.Base;

namespace Hospital.Domain.Entities.QueueItems
{
    public class QueueItem : BaseEntity
    {
        public long DeclarationId { get; set; }
        public int Position { get; set; }
        public int State { get; set; }
        public DateTime Date { get; set; }

    }
}
