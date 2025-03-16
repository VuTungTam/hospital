namespace Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Models
{
    public class DistanceMatrixRequest
    {
        public double SourceLatitude { get; set; }

        public double SourceLongitude { get; set; }

        public double DestinationLatitude { get; set; }

        public double DestinationLongitude { get; set; }

        public int RoundDecimalPlaces { get; set; } = -1;

        public double RoundedSourceLatitude => RoundDecimalPlaces == -1 ? SourceLatitude : Math.Round(SourceLatitude, RoundDecimalPlaces);

        public double RoundedSourceLongitude => RoundDecimalPlaces == -1 ? SourceLongitude : Math.Round(SourceLongitude, RoundDecimalPlaces);

        public string SourceLatLng => $"{RoundedSourceLatitude.ToString().Replace(",", ".")},{RoundedSourceLongitude.ToString().Replace(",", ".")}";

        public string DestinationLatLng => $"{DestinationLatitude.ToString().Replace(",", ".")},{DestinationLongitude.ToString().Replace(",", ".")}";
    }
}
