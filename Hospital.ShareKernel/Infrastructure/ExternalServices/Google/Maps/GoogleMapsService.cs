using Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Models;
using Newtonsoft.Json;

namespace Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Maps
{
    public class GoogleMapsService : IGoogleMapsService
    {
        private readonly IHttpClientFactory _factory;

        public GoogleMapsService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<DistanceMatrixResponse> GetDistanceMatrixAsync(DistanceMatrixRequest request, CancellationToken cancellationToken)
        {
            var client = _factory.CreateClient();
            var url = $"{GoogleMapsConfig.Url}&apikey={GoogleMapsConfig.ApiKey}&point={request.SourceLatLng}&point={request.DestinationLatLng}&sources=0&destinations=1";
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

            var httpResponse = await client.SendAsync(httpRequest, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonConvert.DeserializeObject<GoogleDistanceMatrixResponse>(content);

            return new DistanceMatrixResponse
            {
                RoundedSourceLatitude = request.RoundedSourceLatitude,
                RoundedSourceLongitude = request.RoundedSourceLongitude,
                DestinationLatitude = request.DestinationLatitude,
                DestinationLongitude = request.DestinationLongitude,
                DistanceMeter = response.Distances[0][0],
                Duration = response.Durations[0][0]
            };
        }
    }
}
