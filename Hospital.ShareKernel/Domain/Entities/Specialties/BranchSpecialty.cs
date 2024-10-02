using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Branches;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Specialties
{
    [Table("BranchSpecialty")]
    public class BranchSpecialty : 
        BaseEntity,
        ICreated,
        ICreator,
        ISoftDelete,
        IDeletedBy
    {
        public long BranchId { get; set; }
        public long SpecialtyId { get; set; }
        public Branch Branch { get; set; }
        public Specialty Specialty { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public long? Creator { get; set; }
        public DateTime? Deleted { get; set; }
        public long? DeletedBy { get; set; }
    }
}
