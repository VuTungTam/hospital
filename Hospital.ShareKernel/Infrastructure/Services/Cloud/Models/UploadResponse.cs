using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.SharedKernel.Infrastructure.Services.Cloud.Models
{
    public class UploadResponse : BaseResponse
    {
        public bool Success { get; set; } = true;

        public string OriginFileName { get; set; }

        public string CurrentFileName { get; set; }

        public long Size { get; set; }

        public string FileExtension { get; set; }

        public string Prefix { get; set; }

        public string ErrorMessage { get; set; }

        public string Url { get; set; }

        public UploadResponse(string originFileName, string currentFileName, long size, string fileExtension, string url)
        {
            OriginFileName = originFileName;
            CurrentFileName = currentFileName;
            Size = size;
            FileExtension = fileExtension;
            Url = url;
        }

        public UploadResponse(bool success, string originalFileName, string errorMessage)
        {
            Success = success;
            OriginFileName = originalFileName;
            ErrorMessage = errorMessage;
        }
    }
}
