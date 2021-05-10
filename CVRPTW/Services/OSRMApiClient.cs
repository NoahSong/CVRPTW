using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CVRPTW.Models.OsrmApi;
using CVRPTW.Models.VehicleRouting;
using CVRPTW.Shared;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CVRPTW.Services
{
    public class OsrmApiClient : IOsrmApiClient
    {
        private readonly IOptions<AppSettingsModel> _appSettings;
        private readonly HttpClient _httpClient;
        public OsrmApiClient(IOptions<AppSettingsModel> appSettings)
        {
            _httpClient = new HttpClient();
            _appSettings = appSettings;
        }
        public async Task<int[,]> GetOsrmRoutingMatrixResultAsync(VehicleRoutingModel parameter)
        {
            var origins = new RoutingMatrixRequestModel();
            origins.Origins.Add($"{parameter.Depot.Location.Longitude.ToString()},{parameter.Depot.Location.Latitude.ToString()}" );

            foreach (var pBooking in parameter.Bookings)
                origins.Origins.Add($"{pBooking.Location.Longitude.ToString()},{pBooking.Location.Latitude.ToString()}");
            
            var OSRMMartixUrl = $"{_appSettings.Value.Osrm.RoutingMatrixUrl}{String.Join(";", origins.Origins)}{((origins.exclude != null)? $"?exclude={origins.exclude}" : string.Empty) }";
            var request = new HttpRequestMessage(HttpMethod.Get, OSRMMartixUrl);
            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                // MatrixAttributes
                var result = JsonConvert.DeserializeObject<RoutingMatrixResponseModel>(response.Content.ReadAsStringAsync().Result);
                var matrix = result.durations;
                var lineCount = matrix.Count;
                var columsCount = matrix[0].Count;
                var timeMatrix = new int[lineCount, columsCount];
                for (int i = 0; i < matrix.Count; i++)
                {
                    Buffer.BlockCopy(
                        matrix[i].Select(d => (int)d).ToArray(),
                        0,
                        timeMatrix,
                        i * lineCount * sizeof(int),
                        lineCount * sizeof(int));
                }
                return timeMatrix;
            }
            return null;
        }
    }
}
