using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Dtos.Users;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Enums;
using Ocelot.Values;

namespace Hospital.Application.Dtos.Doctors
{
    public class DotorDto : BaseUserDto
    {
        public string Degree { get; set; }

        public string Expertise { get; set; }
        public List<SpecialtyDto> SpecialtieDtos { get; set; }

        public List<HealthServiceDto> ServiceDtos { get; set; }
        public string Description { get; set; }

        public string TrainingProcess { get; set; }

        public string WorkExperience { get; set; }

        public DoctorStatus DoctorStatus { get; set; }
    }
}
