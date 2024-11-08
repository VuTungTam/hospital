using Hospital.SharedKernel.Infrastructure.Services.Cloud.Models;
namespace Hospital.SharedKernel.Infrastructure.Services.Cloud
{
    public interface ICloudinaryService
    {
        Task<UploadResponse> UploadAsync(UploadRequest model, CancellationToken cancellationToken = default);

        Task<List<UploadResponse>> UploadAsync(List<UploadRequest> models, CancellationToken cancellationToken = default);

        Task<DownloadResponse> DownloadAsync(string fileName, string version = "", CancellationToken cancellationToken = default);

        Task<List<DownloadResponse>> DownloadAsync(List<string> fileNames, string version = "", CancellationToken cancellationToken = default);

        Task DeleteAsync(string fileName, string version = "", CancellationToken cancellationToken = default);
    }
}
