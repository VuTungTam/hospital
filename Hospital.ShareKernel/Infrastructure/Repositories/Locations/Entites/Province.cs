using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites
{
    [Table("location_provinces")]
    public class Province : BaseLocation
    {
        public string Slug { get; set; }
    }
}
