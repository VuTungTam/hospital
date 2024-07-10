using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;

namespace Hospital.Domain.Entities.SocialNetworks
{
    public class SocialNetwork : BaseEntity,
        ICreated,
        ICreator,
        IModified,
        IModifier,
        ISoftDelete
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public string Logo { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public long? Creator { get; set; }

        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }

    }
}
