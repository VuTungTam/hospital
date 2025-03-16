using Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Models;

namespace Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Maps
{
    public interface IGoogleMapsService
    {
        Task<DistanceMatrixResponse> GetDistanceMatrixAsync(DistanceMatrixRequest request, CancellationToken cancellationToken);
    }
}
