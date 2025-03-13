using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Application.Services.Auth.Entities
{
    [Table("mcs_refresh_tokens")]
    public class RefreshToken :
        BaseEntity,
        IOwnedEntity,
        ICreated,
        ICreator,
        IModified,
        IModifier,
        ISoftDelete,
        IDeletedBy
    {
        public string RefreshTokenValue { get; set; }

        public string CurrentAccessToken { get; set; }

        public DateTime ExpiryDate { get; set; }

        public long OwnerId { get; set; }

        public DateTime Created { get; set; }

        public long? Creator { get; set; }

        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }
    }
}
