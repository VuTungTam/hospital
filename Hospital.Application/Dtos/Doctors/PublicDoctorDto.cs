﻿using Hospital.Application.Dtos.Specialties;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Configures.Models;

namespace Hospital.Application.Dtos.Doctors
{
    public class PublicDoctorDto : BaseDto
    {
        public string Name { get; set; }

        public string Avatar { get; set; }

        public string AvatarUrl => !string.IsNullOrEmpty(Avatar) ? CdnConfig.Get(Avatar) : (!string.IsNullOrEmpty(PhotoUrl) ? PhotoUrl : "");

        public string PhotoUrl { get; set; }

        public int Gender { get; set; }

        public DoctorTitle DoctorTitle { get; set; }

        public DoctorDegree DoctorDegree { get; set; }

        public DoctorRank DoctorRank { get; set; }

        public string DoctorTitleStr { get; set; }

        public string DoctorDegreeStr { get; set; }

        public string DoctorRankStr { get; set; }

        public string ExpertiseVn { get; set; }

        public string ExpertiseEn { get; set; }

        public string SpecialtyVns { get; set; }

        public string SpecialtyEns { get; set; }

        public string Description { get; set; }

        public string TrainingProcess { get; set; }

        public string WorkExperience { get; set; }

        public string ProfessionalLevel { get; set; }

        public string CredentialVns { get; set; }

        public string CredentialEns { get; set; }

        public string FacilityId { get; set; }
    }
}
