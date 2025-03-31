using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Dtos.Users;
using Hospital.Domain.Enums;

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

        public List<SpecialtyDto> Specialties { get; set; }

        public string Description { get; set; }

        public string TrainingProcess { get; set; }

        public string WorkExperience { get; set; }

    }
}
