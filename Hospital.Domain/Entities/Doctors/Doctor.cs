﻿using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Constants;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Users;

namespace Hospital.Domain.Entities.Doctors
{
    [Table("mcs_doctors")]
    public class Doctor :
        BaseUser,
        IFacility
    {
        public DoctorTitle DoctorTitle { get; set; }

        public DoctorDegree DoctorDegree { get; set; }

        public DoctorRank DoctorRank { get; set; }

        public string ExpertiseVn { get; set; }

        public string ExpertiseEn { get; set; }

        public List<DoctorSpecialty> DoctorSpecialties { get; set; }

        public List<HealthService> HealthServices { get; set; }

        public long FacilityId { get; set; }
    }
}
