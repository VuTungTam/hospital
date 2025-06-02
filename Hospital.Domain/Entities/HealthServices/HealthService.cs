using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Domain.Entities.HealthServices
{
    [Table("tbl_health_services")]
    public class HealthService
      : BaseEntity,
        IModifiedAt,
        IModifiedBy,
        ICreatedAt,
        ICreatedBy,
        ISoftDelete,
        IDeletedBy,
        IFacility,
        IDoctor
    {
        [Filterable("Name VN")]
        public string NameVn { get; set; }
        [Filterable("Name US")]
        public string NameEn { get; set; }

        public long TypeId { get; set; }

        public ServiceType ServiceType { get; set; }

        public long FacilityId { get; set; }

        public HealthFacility HealthFacility { get; set; }

        public long SpecialtyId { get; set; }

        public Specialty Specialty { get; set; }

        public HealthServiceStatus Status { get; set; }

        public List<Booking> Bookings { get; set; }

        public List<ServiceTimeRule> ServiceTimeRules { get; set; }

        public decimal Price { get; set; }

        public float StarPoint { get; set; }

        public int TotalStars { get; set; }

        public int TotalFeedback { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }

        public Doctor Doctor { get; set; }

        public long DoctorId { get; set; }

        public void getStar()
        {
            StarPoint = TotalStars / TotalFeedback;
        }
    }
}
