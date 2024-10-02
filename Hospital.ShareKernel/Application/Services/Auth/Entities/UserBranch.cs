using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Branches;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Application.Services.Auth.Entities
{
    [Table("UserBranch")]
    public class UserBranch :
        BaseEntity,
        ICreated,
        ICreator,
        ISoftDelete,
        IDeletedBy
    {
        public long BranchId { get; set; }

        public long UserId { get; set; }

        public Branch Branch { get; set; }

        public User User { get; set; }

        public DateTime Created { get; set; }

        public long? Creator { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }
    }
}
