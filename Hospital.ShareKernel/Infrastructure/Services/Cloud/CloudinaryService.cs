using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Configures.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Services.Cloud;
using Hospital.SharedKernel.Infrastructure.Services.Cloud.Models;
using Polly;
using Serilog;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinaryClient;
    private readonly IRedisCache _redisCache;
    private const int expiryTime = 365; // days;

    public CloudinaryService(IRedisCache redisCache)
    {
        _redisCache = redisCache;

        var account = new Account(
            CdnConfig.CloudName,
            CdnConfig.ApiKey,
            CdnConfig.ApiSecret
        );

        _cloudinaryClient = new Cloudinary(account);
    }

    public async Task<UploadResponse> UploadAsync(UploadRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            using (var stream = model.File.OpenReadStream())
            {
                var policy = Policy.Handle<Exception>()
                    .WaitAndRetryAsync(
                        retryCount: 5,
                        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(1, attempt)),
                        onRetry: (exception, timespan, context) =>
                        {
                            Log.Logger.Error($"Cloudinary upload failed for file {model.FileName} with error {exception.Message} at {timespan}");
                        }
                    );

                var currentFileName = Guid.NewGuid().ToString("N");

                // Sử dụng UploadParams để tải lên từ stream
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(model.FileName, stream), // Sử dụng tên tệp và stream
                    PublicId = currentFileName,
                    Overwrite = true
                };

                var uploadResult = await policy.ExecuteAsync(() => _cloudinaryClient.UploadAsync(uploadParams));

                var imageUrl = uploadResult.SecureUrl?.ToString() ?? string.Empty;

                return new UploadResponse(model.FileName, currentFileName, model.Size, model.FileExtension, imageUrl);
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, $"[{model.FileName}] " + ex.Message);
            return new UploadResponse(false, model.FileName, ex.Message);
        }
    }
    public async Task<List<UploadResponse>> UploadAsync(List<UploadRequest> models, CancellationToken cancellationToken = default)
    {
        var responses = await Task.WhenAll(models.Select(model => UploadAsync(model, cancellationToken)));
        return responses.ToList();
    }

    public async Task<DownloadResponse> DownloadAsync(string publicId, string version, CancellationToken cancellationToken = default)
    {
        try
        {
            // Tạo key cho Redis cache
            var key = BaseCacheKeys.GetCloudinaryCacheFileKey(publicId);
            var imageUrl = await _redisCache.GetAsync<string>(key);

            if (!string.IsNullOrEmpty(imageUrl))
            {
                return new DownloadResponse(imageUrl, expiryTime, publicId);
            }

            // Tạo URL hình ảnh từ public ID
            imageUrl = GenerateImageUrl(CdnConfig.CloudName, publicId);

            Log.Logger.Warning($"Download file from Cloudinary with public ID = {publicId}");

            // Lưu URL vào Redis cache
            await _redisCache.SetAsync(key, imageUrl, TimeSpan.FromDays(expiryTime), cancellationToken: cancellationToken);

            return new DownloadResponse(imageUrl, expiryTime, publicId);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, ex.Message);
            throw;
        }
    }
    public string GenerateImageUrl(string YourCloudName, string publicId)
    {
        return $"https://res.cloudinary.com/{YourCloudName}/image/upload/{publicId}.jpg";
    }

    public async Task<List<DownloadResponse>> DownloadAsync(List<string> publicIds, string version = "", CancellationToken cancellationToken = default)
    {
        var tasks = publicIds.Select(id => DownloadAsync(id, version, cancellationToken));
        var responses = await Task.WhenAll(tasks);
        return responses.ToList();
    }


    public async Task DeleteAsync(string publicId, string version = "", CancellationToken cancellationToken = default)
    {
        try
        {
            var deleteParams = new DeletionParams(publicId);
            var deletionResult = await _cloudinaryClient.DestroyAsync(deleteParams);

            if (deletionResult.Result == "ok")
            {
                Log.Logger.Information($"Successfully deleted file with publicId: {publicId}");
            }
            else
            {
                Log.Logger.Warning($"Failed to delete file with publicId: {publicId}, reason: {deletionResult.Result}");
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, ex.Message);
            throw; // Ném lại lỗi để xử lý ở cấp cao hơn nếu cần
        }
    }

   
}
