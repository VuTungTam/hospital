using Microsoft.AspNetCore.Http;

namespace Hospital.SharedKernel.Infrastructure.Services.Cloud.Models
{
    public class UploadRequest
    {
        public IFormFile File { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public long Size { get; set; }

        public UploadRequest(IFormFile file) : this(file, file.FileName)
        {
        }

        public UploadRequest(IFormFile file, string fileName)
        {
            File = file;
            FileName = fileName;
            FileExtension = Path.GetExtension(file.FileName).ToLower();
            Size = file.Length;
        }
    }
}
