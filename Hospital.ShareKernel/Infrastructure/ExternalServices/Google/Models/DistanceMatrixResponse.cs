namespace Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Models
{
    public class DistanceMatrixResponse
    {
        public double RoundedSourceLatitude { get; set; }

        public double RoundedSourceLongitude { get; set; }

        public double DestinationLatitude { get; set; }

        public double DestinationLongitude { get; set; }

        public double DistanceMeter { get; set; }

        public int Duration { get; set; }
    }
}
