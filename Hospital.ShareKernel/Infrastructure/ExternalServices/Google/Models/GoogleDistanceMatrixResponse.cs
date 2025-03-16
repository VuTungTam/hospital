namespace Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Models
{
    public class GoogleDistanceMatrixResponse
    {
        public string Code { get; set; }

        public string Messages { get; set; }

        public int[][] Durations { get; set; }

        public double[][] Distances { get; set; }
    }
}
