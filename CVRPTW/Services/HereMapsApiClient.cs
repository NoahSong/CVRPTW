using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CVRPTW.Models;
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

        public async Task<TourPlanningResponseModel> GetHereMapsTourPlanningResultAsync(VehicleRoutingModel parameter)
        {
            if (parameter != null && parameter.Bookings != null && parameter.Bookings.Length > 0 && parameter.Depot != null && parameter.Depot.Vehicles != null && parameter.Depot.Vehicles.Length > 0)
            {
                var tourPlanningRequest = new TourPlanningRequestModel();

                tourPlanningRequest.Id = Guid.NewGuid().ToString();
                tourPlanningRequest.Configuration = new TourPlanningRequestModel.ConfigurationModel
                {
                    Optimizations = new TourPlanningRequestModel.ConfigurationModel.OptimizationsModel
                    {
                        Traffic = "automatic",
                        WaitingTime = new TourPlanningRequestModel.ConfigurationModel.OptimizationsModel.WaitingTimeModel
                        {
                            BufferTime = 10,
                            Reduce = true
                        }
                    }
                };

                tourPlanningRequest.Fleet = new TourPlanningRequestModel.FleetModel
                {
                    Profiles = new TourPlanningRequestModel.FleetModel.CarProfileModel[] { },
                    Types = new TourPlanningRequestModel.FleetModel.TypeModel[] { }
                };

                var typeList = new List<TourPlanningRequestModel.FleetModel.TypeModel>();
                var profileList = new List<TourPlanningRequestModel.FleetModel.CarProfileModel>();

                foreach (var vehicle in parameter.Depot.Vehicles)
                {
                    List<string> fuelTypes = new List<string>();

                    if ((vehicle.FuelType & FuelType.Petrol) == FuelType.Petrol)
                    {
                        fuelTypes.Add(FuelType.Petrol.ToString());
                    }

                    if ((vehicle.FuelType & FuelType.Diesel) == FuelType.Diesel)
                    {
                        fuelTypes.Add(FuelType.Diesel.ToString());
                    }


                    var vehicleType = new TourPlanningRequestModel.FleetModel.TypeModel
                    {
                        Id = vehicle.Name,
                        Profile = vehicle.Name,
                        Skills = fuelTypes.ToArray(),
                        Capacity = new int[] { 0 },
                        Amount = 1,
                        // Limits = new TourPlanningRequestModel.FleetModel.TypeModel.VehicleLimitsModel
                        // {

                        // },
                        Costs = new TourPlanningRequestModel.FleetModel.TypeModel.CostsModel
                        {
                            Distance = 0,
                            Fixed = 0,
                            Time = 0
                        },
                        Shifts = new TourPlanningRequestModel.FleetModel.TypeModel.VehicleShiftModel[]
                        {
                            new TourPlanningRequestModel.FleetModel.TypeModel.VehicleShiftModel
                            {
                                Start = new TourPlanningRequestModel.FleetModel.TypeModel.VehicleShiftModel.ShiftModel
                                {
                                    Location = new TourPlanningRequestModel.LocationModel
                                    {
                                        Latitude = parameter.Depot.Location.Latitude,
                                        Longitude = parameter.Depot.Location.Longitude
                                    },
                                    Time = vehicle.DeliveryStartTime.ToString("yyyy-MM-ddTHH:mm:ssKZ")
                                },
                                End = new TourPlanningRequestModel.FleetModel.TypeModel.VehicleShiftModel.ShiftModel
                                {
                                    Location = new TourPlanningRequestModel.LocationModel
                                    {
                                        Latitude = parameter.Depot.Location.Latitude,
                                        Longitude = parameter.Depot.Location.Longitude
                                    },
                                    Time = vehicle.DeliveryEndTime.ToString("yyyy-MM-ddTHH:mm:ssKZ")
                                },
                                //Breaks = new TourPlanningRequestModel.FleetModel.TypeModel.VehicleShiftModel.BreakModel[]{ }
                            }
                        }
                    };

                    var vehicleProfile = new TourPlanningRequestModel.FleetModel.CarProfileModel
                    {
                        Name = vehicle.Name,
                        DepartureTime = vehicle.DeliveryStartTime.ToString("yyyy-MM-ddTHH:mm:ssKZ")
                    };

                    typeList.Add(vehicleType);
                    profileList.Add(vehicleProfile);
                }

                tourPlanningRequest.Fleet = new TourPlanningRequestModel.FleetModel
                {
                    Types = typeList.ToArray(),
                    Profiles = profileList.ToArray()
                };

                var jobList = new List<TourPlanningRequestModel.PlanModel.JobModel>();
                foreach (var booking in parameter.Bookings)
                {
                    List<string> fuelTypes = new List<string>();

                    if ((booking.FuelType & FuelType.Petrol) == FuelType.Petrol)
                    {
                        fuelTypes.Add(FuelType.Petrol.ToString());
                    }

                    if ((booking.FuelType & FuelType.Diesel) == FuelType.Diesel)
                    {
                        fuelTypes.Add(FuelType.Diesel.ToString());
                    }

                    var bookingJob = new TourPlanningRequestModel.PlanModel.JobModel
                    {
                        Id = booking.Title,
                        Places = new TourPlanningRequestModel.PlanModel.JobModel.PlaceModel
                        {
                            Deliveries = new TourPlanningRequestModel.PlanModel.JobModel.PlaceModel.DeliveryModel[]
                            {
                                new TourPlanningRequestModel.PlanModel.JobModel.PlaceModel.DeliveryModel
                                {
                                    Location = new TourPlanningRequestModel.LocationModel
                                    {
                                        Latitude = booking.Location.Latitude,
                                        Longitude = booking.Location.Longitude
                                    },
                                    Duration = booking.ServiceMins * 60,
                                    Demand = new int[] {0},
                                    Times = new string[] []
                                    {
                                        new string[] { booking.ServiceFromTime.ToString("yyyy-MM-ddTHH:mm:ssKZ"), booking.ServiceToTime.ToString("yyyy-MM-ddTHH:mm:ssKZ") }
                                    },
                                    //Tag = ""
                                }
                            }
                        },
                        Skills = fuelTypes.ToArray()
                    };

                    jobList.Add(bookingJob);
                }

                tourPlanningRequest.Plan = new TourPlanningRequestModel.PlanModel
                {
                    Jobs = jobList.ToArray()
                };

                var apiKey = _appSettings.Value.HereMaps.ApiKey;

                var hereMapMartixUrl = $"{_appSettings.Value.HereMaps.TourPlanningUrl}?apiKey={apiKey}";
                var request = new HttpRequestMessage(HttpMethod.Post, hereMapMartixUrl);

                request.Content = new StringContent(JsonConvert.SerializeObject(tourPlanningRequest));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var httpClient = new HttpClient();
                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<TourPlanningResponseModel>(response.Content.ReadAsStringAsync().Result);
                    //var result = JsonConvert.DeserializeObject<TourPlanningResponseModel>("{\"statistic\":{\"cost\":0.0,\"distance\":139161,\"duration\":15859,\"times\":{\"driving\":9259,\"serving\":6600,\"waiting\":0,\"break\":0}},\"problemId\":\"6d89dc6a-818c-453a-a57f-362f2c590a84\",\"tours\":[{\"vehicleId\":\"Vehicle1_1\",\"typeId\":\"Vehicle1\",\"stops\":[{\"location\":{\"lat\":-39.2085,\"lng\":173.978},\"time\":{\"arrival\":\"2021-04-05T07:03:37Z\",\"departure\":\"2021-04-05T07:03:37Z\"},\"load\":[0],\"activities\":[{\"jobId\":\"departure\",\"type\":\"departure\"}]},{\"location\":{\"lat\":-39.1996,\"lng\":173.987},\"time\":{\"arrival\":\"2021-04-05T07:06:42Z\",\"departure\":\"2021-04-05T07:41:42Z\"},\"load\":[0],\"activities\":[{\"jobId\":\"4\",\"type\":\"delivery\"}]},{\"location\":{\"lat\":-39.254,\"lng\":174.283},\"time\":{\"arrival\":\"2021-04-05T08:55:01Z\",\"departure\":\"2021-04-05T09:05:01Z\"},\"load\":[0],\"activities\":[{\"jobId\":\"9\",\"type\":\"delivery\"}]},{\"location\":{\"lat\":-39.2008,\"lng\":173.982},\"time\":{\"arrival\":\"2021-04-05T10:16:16Z\",\"departure\":\"2021-04-05T10:41:16Z\"},\"load\":[0],\"activities\":[{\"jobId\":\"5\",\"type\":\"delivery\"}]},{\"location\":{\"lat\":-39.1996,\"lng\":173.995},\"time\":{\"arrival\":\"2021-04-05T10:43:36Z\",\"departure\":\"2021-04-05T11:23:36Z\"},\"load\":[0],\"activities\":[{\"jobId\":\"3\",\"type\":\"delivery\"}]},{\"location\":{\"lat\":-39.2085,\"lng\":173.978},\"time\":{\"arrival\":\"2021-04-05T11:27:56Z\",\"departure\":\"2021-04-05T11:27:56Z\"},\"load\":[0],\"activities\":[{\"jobId\":\"arrival\",\"type\":\"arrival\"}]}],\"statistic\":{\"cost\":0.0,\"distance\":139161,\"duration\":15859,\"times\":{\"driving\":9259,\"serving\":6600,\"waiting\":0,\"break\":0}}}],\"unassigned\":[{\"jobId\":\"2\",\"reasons\":[{\"code\":\"SKILL_CONSTRAINT\",\"description\":\"cannot serve required skill\"}]},{\"jobId\":\"1\",\"reasons\":[{\"code\":\"SKILL_CONSTRAINT\",\"description\":\"cannot serve required skill\"}]},{\"jobId\":\"6\",\"reasons\":[{\"code\":\"SKILL_CONSTRAINT\",\"description\":\"cannot serve required skill\"}]},{\"jobId\":\"7\",\"reasons\":[{\"code\":\"SKILL_CONSTRAINT\",\"description\":\"cannot serve required skill\"}]},{\"jobId\":\"8\",\"reasons\":[{\"code\":\"SKILL_CONSTRAINT\",\"description\":\"cannot serve required skill\"}]},{\"jobId\":\"10\",\"reasons\":[{\"code\":\"SKILL_CONSTRAINT\",\"description\":\"cannot serve required skill\"}]}]}");

                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
