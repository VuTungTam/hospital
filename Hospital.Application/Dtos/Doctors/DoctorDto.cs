using System.Data;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Dtos.Users;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Doctors
{
    public class DoctorDto : BaseUserDto
    {
        public DoctorTitle DoctorTitle { get; set; }

        public DoctorDegree DoctorDegree { get; set; }

        public DoctorRank DoctorRank { get; set; }

        public string DoctorTitleStr { get; set; }

        public string DoctorDegreeStr { get; set; }

        public string DoctorRankStr { get; set; }

        public string Expertise { get; set; }

        public List<SpecialtyDto> Specialties { get; set; } = new List<SpecialtyDto>();

        public List<string> SpecialtyIds { get; set; } = new List<string>();

        public string ListSpecialtyNameVns => Specialties != null && Specialties.Any()
        ? string.Join(", ", Specialties
            .Select(s => s.NameVn))
        : "Chưa có thông tin";


        public string ListSpecialtyNameEns => Specialties != null && Specialties.Any()
            ? string.Join(", ", Specialties.Select(s => s.NameEn))
            : "No data";

        public string Description { get; set; }

        public string TrainingProcess { get; set; }

        public string WorkExperience { get; set; }

        public string ProfessionalLevel { get; set; }
    }
}

