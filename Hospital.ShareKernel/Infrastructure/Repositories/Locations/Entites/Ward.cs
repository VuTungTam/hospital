using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites
{
    [Table("location_wards")]
    public class Ward : BaseLocation
    {
        public long DistrictId { get; set; }
    }
}
