﻿using Hospital.Domain.Entities.Doctors;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Specialties
{
    [Table("tbl_doctor_specialty")]
    public class DoctorSpecialty :
        BaseEntity
    {
        public long DoctorId { get; set; }

        public long SpecialtyId { get; set; }

        public Doctor Doctor { get; set; }

        public Specialty Specialty { get; set; }
    }
}
