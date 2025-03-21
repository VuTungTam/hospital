using Hospital.Application.Repositories.Interfaces.Distances;
using Hospital.Domain.Constants;
using Hospital.Domain.Entities.Distances;
using Hospital.Infrastructure.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Maps;
using Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Runtime.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Serilog;

namespace Hospital.Infrastructure.Repositories.Distances
{
    public class DistanceRepository : WriteRepository<Distance>, IDistanceRepository
    {
        private readonly IGoogleMapsService _googleMapsService;

        public DistanceRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache,
            IGoogleMapsService googleMapsService
        ) : base(serviceProvider, localizer, redisCache)
        {
            _googleMapsService = googleMapsService;
        }

        public async Task<DistanceMatrixResponse> GetDistanceMatrixResponseAsync(DistanceMatrixRequest dmr, CallbackWrapper callbackWrapper, CancellationToken cancellationToken)
        {
            Log.Logger.Debug($"{dmr.SourceLatitude}, {dmr.SourceLongitude}, {dmr.DestinationLatitude}, {dmr.DestinationLongitude}");

            var cacheEntry = AppCacheManager.GetDistanceMatrixCacheEntry(dmr.SourceLatitude, dmr.SourceLongitude, dmr.DestinationLatitude, dmr.DestinationLongitude);
            var valueFactory = async () =>
            {
                var dm = await _dbSet.FirstOrDefaultAsync(x =>
                    x.SourceLatitude == dmr.RoundedSourceLatitude &&
                    x.SourceLongitude == dmr.RoundedSourceLongitude &&
                    x.DestinationLatitude == dmr.DestinationLatitude &&
                    x.DestinationLongitude == dmr.DestinationLongitude,
                 cancellationToken);

                if (dm == null)
                {
                    var response = await _googleMapsService.GetDistanceMatrixAsync(dmr, cancellationToken);
                    if (response == null)
                    {
                        throw new CatchableException("DistanceMessage.ErrorWhenCalculateDistance");
                    }

                    dm = new Distance
                    {
                        SourceLatitude = response.RoundedSourceLatitude,
                        SourceLongitude = response.RoundedSourceLongitude,
                        DestinationLatitude = response.DestinationLatitude,
                        DestinationLongitude = response.DestinationLongitude,
                        DistanceMeter = response.DistanceMeter,
                        Duration = response.Duration,
                    };

                    await _dbSet.AddAsync(dm, cancellationToken);

                    callbackWrapper.Callback = () => _dbContext.CommitAsync(cancellationToken: cancellationToken);
                }

                return new DistanceMatrixResponse
                {
                    RoundedSourceLatitude = dm.SourceLatitude,
                    RoundedSourceLongitude = dm.SourceLongitude,
                    DestinationLatitude = dm.DestinationLatitude,
                    DestinationLongitude = dm.DestinationLongitude,
                    DistanceMeter = dm.DistanceMeter,
                    Duration = dm.Duration,
                };
            };

            return await _redisCache.GetOrSetAsync(cacheEntry.Key, valueFactory, TimeSpan.FromSeconds(cacheEntry.ExpiriesInSeconds), cancellationToken: cancellationToken);
        }
    }
}
