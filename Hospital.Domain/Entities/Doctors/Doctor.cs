using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Constants;
using Hospital.SharedKernel.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Doctors
{
    [Table("mcs_doctors")]
    public class Doctor : BaseUser
    {
        public string Degree { get; set; }

        public string Expertise { get; set; }
        public List<DoctorSpecialty> DoctorSpecialties { get; set; }

        public List<ServiceDoctor> ServiceDoctors { get; set; }
        public string Description { get; set; }

        public string TrainingProcess { get; set; }

        public string WorkExperience { get; set; }

        public DoctorStatus DoctorStatus { get; set; }
    }
}
