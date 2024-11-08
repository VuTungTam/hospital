using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.SocialNetworks
{
    [Table("SocialNetworks")]
    public class SocialNetwork : BaseEntity,

        ICreated,
        ICreator,
        IModified,
        IModifier,
        ISoftDelete,
        IDeletedBy
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public string Logo { get; set; }

        public string Qr { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public long? Creator { get; set; }

        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }

    }
}
