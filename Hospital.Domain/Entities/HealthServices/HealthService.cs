﻿using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

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
        IDeletedBy
    {
        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public string DescriptionVn { get; set; }

        public string DescriptionEn { get; set; }

        public long TypeId { get; set; }

        public ServiceType ServiceType { get; set; }

        public long FacilitySpecialtyId { get; set; }

        public FacilitySpecialty FacilitySpecialty { get; set; }

        public HealthServiceStatus Status { get; set; }

        public List<Booking> Bookings { get; set; }

        public List<ServiceDoctor> ServiceDoctors { get; set; }

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

        public List<ServiceTimeRule> TimeRules { get; set; }

        public void getStar()
        {
            StarPoint = TotalStars / TotalFeedback;
        }

    }
}
