namespace Hospital.SharedKernel.Infrastructure.Services.Cloud.Models
{
    public class DownloadResponse
    {
        public string PresignedUrl { get; set; }

        public int ExpiryTime { get; set; }

        public string FileName { get; set; }

        public DownloadResponse()
        {
        }

        public DownloadResponse(string presignedUrl, int expiryTime, string fileName)
        {
            PresignedUrl = presignedUrl;
            ExpiryTime = expiryTime;
            FileName = fileName;
        }
    }
}
