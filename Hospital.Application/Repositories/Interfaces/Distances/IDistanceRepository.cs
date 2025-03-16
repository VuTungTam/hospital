using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Models;

namespace Hospital.Application.Repositories.Interfaces.Distances
{
    public interface IDistanceRepository
    {
        Task<DistanceMatrixResponse> GetDistanceMatrixResponseAsync(DistanceMatrixRequest dmr, CallbackWrapper callbackWrapper, CancellationToken cancellationToken);
    }
}
