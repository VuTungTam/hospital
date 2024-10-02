using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites
{
    [Table("location_districts")]
    public class District : BaseLocation
    {
        public long ProvinceId { get; set; }
    }
}
