using Hospital.Domain.Entities.Doctors;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.HealthServices
{
    [Table("tbl_service_doctor")]
    public class ServiceDoctor :
        BaseEntity
    {
        public long ServiceId { get; set; }

        public long DoctorId { get; set; }

        public HealthService Service { get; set; }

        public Doctor Doctor { get; set; }
    }
}
