using Hospital.Domain.Entities.HeathServices;
using Hospital.Domain.Entities.Specialties;
using Hospital.SharedKernel.Application.Service.Auth.Entities;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.HeathFacilities
{
    [Table("HealthFacilities")]
    public class HealthFacility
      : BaseEntity,
        IModified,
        IModifier,
        ICreated,
        ICreator,
        ISoftDelete,
        IDeletedBy
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public long CategoryId { get; set; }
        public FacilityCategory Category { get; set; }
        public List<FacilitySpecialty> FacilitySpecialties { get; set; }
        public int Pid { get; set; }
        public string Pname { get; set; }
        public int Did { get; set; }
        public string Dname { get; set; }
        public int Wid { get; set; }
        public string Wname { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }
        public DateTime? Modified { get; set; }
        public long? Modifier { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public long? Creator { get; set; }
        public DateTime? Deleted { get; set; }
        public long? DeletedBy { get; set; }
    }
}
