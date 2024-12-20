﻿using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.HealthServices
{
    [Table("HealthServices")]
    public class HealthService
      : BaseEntity,
        IModified,
        IModifier,
        ICreated,
        ICreator,
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

        public decimal Price { get; set; }

        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public long? Creator { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }

        public List<ServiceTimeRule> TimeRules { get; set; }
    }
}
