using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Distances
{
    [Table("tbl_distances")]
    public class Distance :
        BaseEntity,
        ICreatedAt
    {
        public double SourceLatitude { get; set; }

        public double SourceLongitude { get; set; }

        public double DestinationLatitude { get; set; }

        public double DestinationLongitude { get; set; }

        public double DistanceMeter { get; set; }

        public int Duration { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
