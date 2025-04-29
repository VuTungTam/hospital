using System.ComponentModel.DataAnnotations.Schema;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;

namespace Hospital.Domain.Entities.HealthServices
{
    [Table("tbl_service_types")]
    public class ServiceType
       : BaseEntity
    {
        public string Logo { get; set; }

        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public string DescriptionVn { get; set; }

        public string DescriptionEn { get; set; }

        public string Image { get; set; }

        public List<HealthService> Services { get; set; }

        public string Slug { get; set; }
    }
}
