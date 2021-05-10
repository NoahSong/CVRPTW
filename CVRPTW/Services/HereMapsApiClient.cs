using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CVRPTW.Models.HereMapsApi;
using CVRPTW.Models.VehicleRouting;
using CVRPTW.Shared;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CVRPTW.Services
{
    public class HereMapsApiClient : IHereMapsApiClient
    {
        private readonly IOptions<AppSettingsModel> _appSettings;
        private readonly HttpClient _httpClient;
        public HereMapsApiClient(IOptions<AppSettingsModel> appSettings)
        {
            _httpClient = new HttpClient();
            _appSettings = appSettings;
        }
        public async Task<RoutingMatrixResultModel> GetHereMapsRoutingMatrixResultAsync(VehicleRoutingModel parameter)
        {
            var origins = new List<RoutingMatrixRequestModel.Coordinates>();
            origins.Add(new RoutingMatrixRequestModel.Coordinates
            {
                Latitude = parameter.Depot.Location.Latitude,
                Longitude = parameter.Depot.Location.Longitude
            });

            origins.AddRange(parameter.Bookings.Select((b, index) =>
            {
                return new RoutingMatrixRequestModel.Coordinates
                {
                    Latitude = b.Location.Latitude,
                    Longitude = b.Location.Longitude
                };
            }));

            var model = new RoutingMatrixRequestModel
            {
                Origins = origins.ToArray(),
                RegionDefinition = new RoutingMatrixRequestModel.RegionModel()
            };

            var apiKey = _appSettings.Value.HereMaps.ApiKey;

            var hereMapMartixUrl = $"{_appSettings.Value.HereMaps.RoutingMatrixUrl}?apiKey={apiKey}&async=false&profile=carShort&routingMode=short";
            var request = new HttpRequestMessage(HttpMethod.Post, hereMapMartixUrl);

            request.Content = new StringContent(JsonConvert.SerializeObject(model));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<RoutingMatrixResponseModel>(response.Content.ReadAsStringAsync().Result);
                var matrix = result.Matrix;
                var timeMatrix = new int[matrix.NumOrigins, matrix.NumDestinations];

                for (int i = 0; i < matrix.NumOrigins; i++)
                {
                    Buffer.BlockCopy(matrix.TravelTimes, i * matrix.NumDestinations * sizeof(int), timeMatrix, i * matrix.NumDestinations * sizeof(int), matrix.NumDestinations * sizeof(int));
                }

                return new RoutingMatrixResultModel
                {
                    Matrix = timeMatrix,
                    Center = new RoutingMatrixResultModel.LocationModel
                    {
                        Lat = result.RegionDefinition.Center.Lat,
                        Lng = result.RegionDefinition.Center.Lng,
                    },
                    Radius = result.RegionDefinition.Radius
                };
            }
            return null;
        }
    }
}
