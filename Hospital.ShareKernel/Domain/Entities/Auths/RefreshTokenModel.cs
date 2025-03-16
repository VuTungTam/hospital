using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Domain.Entities.Auths
{
    [Table("mcs_refresh_tokens")]
    public class RefreshToken :
        BaseEntity,
        IOwnedEntity,
        ICreatedAt,
        ICreatedBy,
        IModifiedAt,
        IModifiedBy,
        ISoftDelete,
        IDeletedBy
    {
        public string RefreshTokenValue { get; set; }

        public string CurrentAccessToken { get; set; }

        public DateTime ExpiryDate { get; set; }

        public long OwnerId { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }
    }
}
