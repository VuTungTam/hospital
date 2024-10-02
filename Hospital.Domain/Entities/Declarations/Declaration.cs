using Hospital.Domain.Entities.Visits;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Declarations
{
    [Table("Declarations")]
    public class Declaration
        : BaseEntity,
        IModified,
        IModifier,
        ICreated,
        ICreator,
        ISoftDelete,
        IDeletedBy
    {
        public string CICode { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int Gender { get; set; }
        public DateTime Dob { get; set; }
        public int Pid { get; set; }
        public string Pname { get; set; }
        public int Did { get; set; }
        public string Dname { get; set; }
        public int Wid { get; set; }
        public string Wname { get; set; }
        public string Address { get; set; }
        public List<Visit> Visits { get; set; }
        public DateTime? Modified { get; set; }
        public long? Modifier { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public long? Creator { get; set; }
        public DateTime? Deleted { get; set; }
        public long? DeletedBy { get; set; }
    }
}
