using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CVRPTW.Models.tomtomApi;
using CVRPTW.Models.VehicleRouting;
using CVRPTW.Shared;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CVRPTW.Services
{
    public class TomtomMapsApiClient : ITomtomMapsApiClient
    {
        private readonly IOptions<AppSettingsModel> _appSettings;
        private readonly HttpClient _httpClient;
        public TomtomMapsApiClient(IOptions<AppSettingsModel> appSettings)
        {
            _httpClient = new HttpClient();
            _appSettings = appSettings;
        }
        public async Task<int[,]> GetTomtomRoutingMatrixResultAsync(VehicleRoutingModel parameter)
        {
            int count = 0;
            var origins = new List<RoutingMatrixRequestModel.Origin>();
            origins.Add(new RoutingMatrixRequestModel.Origin
            {
                point = new RoutingMatrixRequestModel.Point { 
                    latitude = parameter.Depot.Location.Latitude,
                    longitude = parameter.Depot.Location.Longitude
                }
            });
            origins.AddRange(parameter.Bookings.Select((b, index) =>
            {
                return new RoutingMatrixRequestModel.Origin
                {
                    point = new RoutingMatrixRequestModel.Point
                    {
                        latitude = b.Location.Latitude,
                        longitude = b.Location.Longitude
                    }
                };
            }));

            var destinations = new List<RoutingMatrixRequestModel.Destination>();
            destinations.Add(new RoutingMatrixRequestModel.Destination
            {
                point = new RoutingMatrixRequestModel.Point
                {
                    latitude = parameter.Depot.Location.Latitude,
                    longitude = parameter.Depot.Location.Longitude
                }
            });
            destinations.AddRange(parameter.Bookings.Select((b, index) =>
            {
                return new RoutingMatrixRequestModel.Destination
                {
                    point = new RoutingMatrixRequestModel.Point
                    {
                        latitude = b.Location.Latitude,
                        longitude = b.Location.Longitude
                    }
                };
            }));
            var options = new RoutingMatrixRequestModel.Options() {
                post = new RoutingMatrixRequestModel.Post() {
                    avoidVignette = new List<string>() {
                    "AUS","CHE"
                    }  
                }
            };
            var model = new RoutingMatrixRequestModel
            {
                origins = origins,
                destinations = destinations,
                options = options
            };
            var apiKey = _appSettings.Value.Tomtom.ApiKey;
            var tomtomMapMartixUrl = $"{_appSettings.Value.Tomtom.RoutingMatrixUrl}?key={apiKey}";
            var request = new HttpRequestMessage(HttpMethod.Post, tomtomMapMartixUrl);
            request.Content = new StringContent(JsonConvert.SerializeObject(model));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<RoutingMatrixResponseModel>(response.Content.ReadAsStringAsync().Result);
                var matrix = result.matrix;
                var timeMatrix = new int[matrix.Count, matrix[0].Count];
                for (int i = 0; i < matrix.Count; i++)
                    for (int j = 0; j < matrix[i].Count; j++)
                        timeMatrix[i, j] = matrix[i][j].response.routeSummary.travelTimeInSeconds;
                return timeMatrix;
            }
            return null;
        }
    }
}
