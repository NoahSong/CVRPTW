using CVRPTW.Models;
using CVRPTW.Models.EsriApi;
using CVRPTW.Models.VehicleRouting;
using CVRPTW.Services.Interfaces;
using CVRPTW.Shared;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CVRPTW.Models.EsriApi.RoutingMatrixRequestModel;

namespace CVRPTW.Services
{
    public class EsriApiClient : IEsriApiClient
    {
        private readonly IOptions<AppSettingsModel> _appSettings;
        private readonly HttpClient _httpClient;
        public EsriApiClient(IOptions<AppSettingsModel> appSettings)
        {
            _httpClient = new HttpClient();
            _appSettings = appSettings;
        }
        public async Task<int[,]> GetEsriMapsRoutingMatrixResultAsync(VehicleRoutingModel parameter)
        {
            var routing_matrix_model = new RoutingMatrixRequestModel();
            var origins = new Features() { FeaturesList = new List<Feature>() };
            origins.FeaturesList.Add(
                new Feature()
                {
                    geometry = new Coordinates()
                    {
                        Latitude = parameter.Depot.Location.Latitude,
                        Longitude = parameter.Depot.Location.Longitude
                    }
                });

            origins.FeaturesList.AddRange(parameter.Bookings.Select((b, index) =>
            {
                return new Feature()
                {
                    geometry = new Coordinates()
                    {
                        Latitude = b.Location.Latitude,
                        Longitude = b.Location.Longitude
                    }
                };
            }));

            var model = new RoutingMatrixRequestModel
            {
                Origins = origins,
                Destinations = new Features(),
                Token = _appSettings.Value.Esri.ApiKey
            };

            var apiKey = _appSettings.Value.Esri.ApiKey;

            var hereMapMartixUrl = _appSettings.Value.Esri.RoutingMatrixUrl;
            var jobStatusURL = _appSettings.Value.Esri.JobStatusUrl;
            var request = new HttpRequestMessage(HttpMethod.Post, hereMapMartixUrl);

            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(apiKey), "token");
            form.Add(new StringContent(JsonConvert.SerializeObject(model.Origins)), "origins");
            form.Add(new StringContent(JsonConvert.SerializeObject(model.Origins)), "destinations");
            form.Add(new StringContent(model.RoutingMode), "routingMode");
            form.Add(new StringContent(model.Format), "f");
            form.Add(new StringContent(model.TravelMode), "travel_mode");
            form.Add(new StringContent(model.TimeUnits), "Time_Units");

            request.Content = new StringContent(JsonConvert.SerializeObject(model));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(hereMapMartixUrl, form);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<ApiJobResponse>(response.Content.ReadAsStringAsync().Result);
                if (result.JobStatus == "esriJobSubmitted")
                {
                    var job_status_url = jobStatusURL + "/" + result.JobId + "/?token=" + apiKey + "&returnMessages=true&f=json";
                    // while loop to get the response
                    bool job_success = false;

                    while (job_success == false)
                    {
                        Thread.Sleep(1000);
                        var job_response = await httpClient.GetAsync(job_status_url);
                        if (job_response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var job_result = JsonConvert.DeserializeObject<ApiJobResponse>(job_response.Content.ReadAsStringAsync().Result);
                            if (job_result.JobStatus == "esriJobFailed")
                                job_success = true;
                            else if (job_result.JobStatus == "esriJobSucceeded")
                            {
                                job_success = true;
                                // get origin destination lines 
                                var od_lines_url = jobStatusURL + "/" + result.JobId + "/results/Output_Origin_Destination_Lines/?token=" + apiKey + "&returnMessages=true&f=json";
                                var od_response = await httpClient.GetAsync(od_lines_url);
                                if (od_response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var od_result = JsonConvert.DeserializeObject<OriginDestinationLines>(od_response.Content.ReadAsStringAsync().Result);
                                    var features = od_result.value.features;

                                    var timeMatrix = new int[origins.FeaturesList.Count, origins.FeaturesList.Count];
                                    var lineCount = origins.FeaturesList.Count;
                                    var columsCount = origins.FeaturesList.Count;

                                    for (int l = 0; l < lineCount; l++)
                                    {
                                        int col = 0;
                                        features.Where(c => c.attributes.OriginOID == (l + 1)).OrderBy(c => c.attributes.DestinationOID).ToList().ForEach(c =>
                                        {
                                            timeMatrix[l, col] = (int)Math.Round(c.attributes.Total_Time);
                                            col++;
                                        });

                                    }
                                    return timeMatrix;
                                }
                            }
                        }
                    }

                }
            }
            return null;
        }
    }
}
