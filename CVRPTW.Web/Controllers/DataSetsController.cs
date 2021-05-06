using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CVRPTW.Datasets;
using CVRPTW.Models.VehicleRouting;
using CVRPTW.Services;
using Google.OrTools.ConstraintSolver;
using Microsoft.AspNetCore.Mvc;

namespace CVRPTW.Web.Controllers
{
    public class DataSetsController : Controller
    {
        private readonly IHereMapsApiClient _hereMapsClient;
        public DataSetsController(IHereMapsApiClient hereMapsClient)
        {
            _hereMapsClient = hereMapsClient;
        }

        private void GetTimeWindowsAndServiceTimeMatrix(VehicleRoutingModel dataset, out int[,] timeWindows, out int[] serviceTimeMatrix)
        {
            var serviceTimes = dataset.Bookings.Select(x => x.ServiceMins * 60).ToList();
            serviceTimes.Insert(0, 0);
            serviceTimeMatrix = serviceTimes.ToArray();

            timeWindows = new int[dataset.Bookings.Length + 1, 2];
            timeWindows[0, 0] = (int)dataset.Depot.Vehicles.First().DeliveryStartTime.TotalMinutes * 60;
            timeWindows[0, 1] = (int)dataset.Depot.Vehicles.First().DeliveryEndTime.TotalMinutes * 60;

            for (int i = 1; i <= dataset.Bookings.Length; i++)
            {
                timeWindows[i, 0] = (int)dataset.Bookings[i - 1].ServiceFromTime.TimeOfDay.TotalMinutes * 60;
                timeWindows[i, 1] = (int)dataset.Bookings[i - 1].ServiceToTime.TimeOfDay.TotalMinutes * 60;
            }
        }

        private VehicleRoutingModel SetTimeMatrixAndDurationForEachLocation(VehicleRoutingModel dataset, int[,] timeMatrix)
        {
            dataset.Depot.TimeMatrix = Enumerable.Range(0, timeMatrix.GetLength(1)).Select(x => timeMatrix[0, x]).ToArray();
            dataset.Depot.Points = dataset.Bookings.Select((b, Index) =>
            {
                return new VehicleRoutingModel.Point
                {
                    Latitude = b.Location.Latitude,
                    Longitude = b.Location.Longitude,
                    Duration = dataset.Depot.TimeMatrix[Index + 1]
                };
            }).ToList();

            for (int i = 0; i < dataset.Bookings.Length; i++)
            {
                dataset.Bookings[i].TimeMatrix = Enumerable.Range(0, timeMatrix.GetLength(1)).Select(x => timeMatrix[i + 1, x]).ToArray();
                dataset.Bookings[i].Points = new List<VehicleRoutingModel.Point>();

                for (int j = i + 1; j < dataset.Bookings.Length; j++)
                {
                    dataset.Bookings[i].Points.Add(new VehicleRoutingModel.Point
                    {
                        Latitude = dataset.Bookings[j].Location.Latitude,
                        Longitude = dataset.Bookings[j].Location.Longitude,
                        Duration = dataset.Bookings[i].TimeMatrix[j]
                    });
                }
            }
            return dataset;
        }

        private VehicleRoutingModel SetFoundSolution(VehicleRoutingModel dataset, RoutingModel routing, RoutingIndexManager manager, Assignment solution)
        {
            for (int i = 0; i < dataset.Depot.Vehicles.Length; i++)
            {
                dataset.Depot.Vehicles[i].TotalDuration = 0;
                int order = 0;
                var index = routing.Start(i);
                var ordinalBookings = new List<VehicleRoutingModel.BookingModel>();

                while (routing.IsEnd(index) == false)
                {
                    var nodeIndex = manager.IndexToNode(index);
                    var previousIndex = index;

                    index = solution.Value(routing.NextVar(index));

                    if (nodeIndex == 0) // depot
                    {
                        dataset.Depot.NextNodeIndex = index - 1;
                        order++;
                    }
                    else
                    {
                        dataset.Bookings[nodeIndex - 1].NextNodeIndex = index - 1;
                        dataset.Bookings[nodeIndex - 1].Order = order++;

                        ordinalBookings.Add(dataset.Bookings[nodeIndex - 1]);

                        dataset.Depot.Vehicles[i].TotalDuration += routing.GetArcCostForVehicle(previousIndex, index - 1, i);
                    }
                }

                dataset.Depot.Vehicles[i].OrdinalBookings = ordinalBookings.ToArray();
                dataset.TotalDuration += dataset.Depot.Vehicles[i].TotalDuration;
            }

            return dataset;
        }

        [HttpGet, Route("api/datasets/test-vrp")]
        public async Task<VehicleRoutingModel> RunVRPTestData()
        {
            var initialDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var petrol = FuelType.Petrol.ToString();
            var diesel = FuelType.Diesel.ToString();

            #region dataset
            var dataset = new VehicleRoutingModel
            {
                Bookings = new[]
                {
                    new VehicleRoutingModel.BookingModel {Title = "1", Location = new VehicleRoutingModel.Location{ Latitude = -43.5247272, Longitude = 172.6335131 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "2", Location = new VehicleRoutingModel.Location{ Latitude = -43.5175961, Longitude = 172.6059798 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "3", Location = new VehicleRoutingModel.Location{ Latitude = -43.5334978, Longitude = 172.6401938 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "4", Location = new VehicleRoutingModel.Location{ Latitude = -43.4991287, Longitude = 172.5594919 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "5", Location = new VehicleRoutingModel.Location{ Latitude = -43.5165243, Longitude = 172.5784314 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "6", Location = new VehicleRoutingModel.Location{ Latitude = -43.4863798, Longitude = 172.5519917 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "7", Location = new VehicleRoutingModel.Location{ Latitude = -43.5023704, Longitude = 172.5578182 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "8", Location = new VehicleRoutingModel.Location{ Latitude = -43.5026636, Longitude = 172.6100076 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "9", Location = new VehicleRoutingModel.Location{ Latitude = -43.4744394, Longitude = 172.5952965 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "10", Location = new VehicleRoutingModel.Location{ Latitude = -43.491454, Longitude = 172.5696854 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "11", Location = new VehicleRoutingModel.Location{ Latitude = -43.518623, Longitude = 172.5993218 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "12", Location = new VehicleRoutingModel.Location{ Latitude = -43.517315, Longitude = 172.6256849 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "13", Location = new VehicleRoutingModel.Location{ Latitude = -43.521171, Longitude = 172.6337905 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "14", Location = new VehicleRoutingModel.Location{ Latitude = -43.518567, Longitude = 172.5289902 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "15", Location = new VehicleRoutingModel.Location{ Latitude = -43.490456, Longitude = 172.5931206 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "16", Location = new VehicleRoutingModel.Location{ Latitude = -43.526242, Longitude = 172.5174277 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "17", Location = new VehicleRoutingModel.Location{ Latitude = -43.509325, Longitude = 172.5573584 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "18", Location = new VehicleRoutingModel.Location{ Latitude = -43.484281, Longitude = 172.5794435 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "19", Location = new VehicleRoutingModel.Location{ Latitude = -43.499813, Longitude = 172.6292605 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "20", Location = new VehicleRoutingModel.Location{ Latitude = -43.506631, Longitude = 172.6141748 }, FuelType = FuelType.Petrol },
                    new VehicleRoutingModel.BookingModel {Title = "21", Location = new VehicleRoutingModel.Location{ Latitude = -43.523728, Longitude = 172.5811548 }, FuelType = FuelType.Petrol },
                },
                Depot = new VehicleRoutingModel.DepotModel
                {
                    Vehicles = new[]
                    {
                        new VehicleRoutingModel.DepotModel.Vehicle
                        {
                            Name = "Vehicle 1",
                            FuelType = FuelType.Diesel | FuelType.Petrol
                        }
                    },
                    Location = new VehicleRoutingModel.Location
                    {
                        Latitude = -43.55858710975329,
                        Longitude = 172.51475521406968
                    }
                }
            };
            #endregion

            var timeMatrix = await _hereMapsClient.GetHereMapsRoutingMatrixResultAsync(dataset);

            if (timeMatrix != null && timeMatrix.GetLength(1) == dataset.Bookings.Length + 1)
            {
                dataset.Depot.TimeMatrix = Enumerable.Range(0, timeMatrix.GetLength(1)).Select(x => timeMatrix[0, x]).ToArray();
                dataset.Depot.Points = dataset.Bookings.Select((b, Index) =>
                {
                    return new VehicleRoutingModel.Point
                    {
                        Latitude = b.Location.Latitude,
                        Longitude = b.Location.Longitude,
                        Duration = dataset.Depot.TimeMatrix[Index + 1]
                    };
                }).ToList();

                for (int i = 0; i < dataset.Bookings.Length; i++)
                {
                    dataset.Bookings[i].TimeMatrix = Enumerable.Range(0, timeMatrix.GetLength(1)).Select(x => timeMatrix[i + 1, x]).ToArray();
                    dataset.Bookings[i].Points = new List<VehicleRoutingModel.Point>();

                    for (int j = i + 1; j < dataset.Bookings.Length; j++)
                    {
                        dataset.Bookings[i].Points.Add(new VehicleRoutingModel.Point
                        {
                            Latitude = dataset.Bookings[j].Location.Latitude,
                            Longitude = dataset.Bookings[j].Location.Longitude,
                            Duration = dataset.Bookings[i].TimeMatrix[j]
                        });
                    }
                }

                var manager = new RoutingIndexManager(timeMatrix.GetLength(0), 1, 0);
                var routing = new RoutingModel(manager);

                int transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
                {
                    var fromNode = manager.IndexToNode(fromIndex);
                    var toNode = manager.IndexToNode(toIndex);
                    return timeMatrix[fromNode, toNode];
                });


                routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);
                var searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
                searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;

                var solution = routing.SolveWithParameters(searchParameters);
                dataset.TotalDuration = solution.ObjectiveValue();

                var index = routing.Start(0);
                int order = 0;
                var ordinalBookings = new List<VehicleRoutingModel.BookingModel>();

                while (routing.IsEnd(index) == false)
                {
                    var nodeIndex = manager.IndexToNode(index);
                    var previousIndex = index;

                    index = solution.Value(routing.NextVar(index));

                    if (nodeIndex == 0) // depot
                    {
                        dataset.Depot.NextNodeIndex = index - 1;
                        order++;
                    }
                    else
                    {
                        dataset.Bookings[nodeIndex - 1].NextNodeIndex = index - 1;
                        dataset.Bookings[nodeIndex - 1].Order = order++;
                        ordinalBookings.Add(dataset.Bookings[nodeIndex - 1]);

                        dataset.Depot.Vehicles[0].TotalDuration += routing.GetArcCostForVehicle(previousIndex, index - 1, 0);
                    }
                }

                dataset.Depot.Vehicles[0].OrdinalBookings = ordinalBookings.ToArray();
                dataset.TotalDuration = dataset.Depot.Vehicles[0].TotalDuration;
            }

            return dataset;
        }

        [HttpGet, Route("api/datasets/test-vrptw")]
        public async Task<VehicleRoutingModel> RunVRPTWTestData()
        {
            var initialDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);
            var petrol = FuelType.Petrol.ToString();
            var diesel = FuelType.Diesel.ToString();

            var dataset = new VehicleRoutingModel
            {
                Bookings = new[]
                {
                    new VehicleRoutingModel.BookingModel { Title = "1", Location =  new VehicleRoutingModel.Location { Latitude = -43.5584496, Longitude = 172.5143579 },  FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(2), ServiceMins = 20 }, // 06~08
                    new VehicleRoutingModel.BookingModel { Title = "2", Location =  new VehicleRoutingModel.Location { Latitude = -43.5415601, Longitude = 172.5723066 },  FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(2), ServiceMins = 30 }, // 06~08
                    new VehicleRoutingModel.BookingModel { Title = "3", Location =  new VehicleRoutingModel.Location { Latitude = -43.5334978, Longitude = 172.5641035 },  FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(2), ServiceMins = 30 }, // 06~08
                    new VehicleRoutingModel.BookingModel { Title = "4", Location =  new VehicleRoutingModel.Location { Latitude = -43.5383194, Longitude = 172.6492987 },  FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(2), ServiceMins = 10 }, // 06~08
                    new VehicleRoutingModel.BookingModel { Title = "5", Location =  new VehicleRoutingModel.Location { Latitude = -43.5340853, Longitude = 172.6950710 },  FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(2), ServiceMins = 10 }, // 06~08
                    new VehicleRoutingModel.BookingModel { Title = "6", Location =  new VehicleRoutingModel.Location { Latitude = -43.5580008, Longitude = 172.5165407 },  FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(12), ServiceMins = 40 }, // 08-10
                    new VehicleRoutingModel.BookingModel { Title = "7", Location =  new VehicleRoutingModel.Location { Latitude = -43.5398800, Longitude = 172.6951982 },  FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(2), ServiceToTime = initialDateTime.AddHours(4), ServiceMins = 20 }, // 08-10
                    new VehicleRoutingModel.BookingModel { Title = "8", Location =  new VehicleRoutingModel.Location { Latitude = -43.5382537,  Longitude = 172.631427 },  FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(4), ServiceMins = 30 }, // 08-10
                    new VehicleRoutingModel.BookingModel { Title = "9", Location =  new VehicleRoutingModel.Location { Latitude = -43.53599324, Longitude = 172.6412657 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(2), ServiceToTime = initialDateTime.AddHours(4), ServiceMins = 30 }, // 08-10
                    new VehicleRoutingModel.BookingModel { Title = "10", Location = new VehicleRoutingModel.Location { Latitude = -43.51444218, Longitude = 172.5999140 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(2), ServiceToTime = initialDateTime.AddHours(4), ServiceMins = 15 }, // 08-10
                    new VehicleRoutingModel.BookingModel { Title = "11", Location = new VehicleRoutingModel.Location { Latitude = -43.60275960, Longitude = 172.7174189 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(4), ServiceToTime = initialDateTime.AddHours(6), ServiceMins = 10 }, // 13-15
                    new VehicleRoutingModel.BookingModel { Title = "12", Location = new VehicleRoutingModel.Location { Latitude = -43.57462860, Longitude = 172.7509300 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(4), ServiceToTime = initialDateTime.AddHours(6), ServiceMins = 25 }, // 13-15
                    new VehicleRoutingModel.BookingModel { Title = "13", Location = new VehicleRoutingModel.Location { Latitude = -43.54230760, Longitude = 172.5935746 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(4), ServiceToTime = initialDateTime.AddHours(6), ServiceMins = 40 }, // 14-16
                    new VehicleRoutingModel.BookingModel { Title = "14", Location = new VehicleRoutingModel.Location { Latitude = -43.53526933, Longitude = 172.2954649 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(6), ServiceMins = 30 }, // 14-16
                    new VehicleRoutingModel.BookingModel { Title = "15", Location = new VehicleRoutingModel.Location { Latitude = -43.53446700,  Longitude = 172.539369 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(6), ServiceMins = 60 }, // 14-16
                    new VehicleRoutingModel.BookingModel { Title = "16", Location = new VehicleRoutingModel.Location { Latitude = -43.51218440, Longitude = 172.5027829 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(6), ServiceMins = 20 }, // 15-16
                    new VehicleRoutingModel.BookingModel { Title = "17", Location = new VehicleRoutingModel.Location { Latitude = -43.50909426, Longitude = 172.3271377 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(6), ServiceMins = 15 }, // 15-18
                    new VehicleRoutingModel.BookingModel { Title = "18", Location = new VehicleRoutingModel.Location { Latitude = -43.50586640, Longitude = 172.6296308 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(6), ServiceMins = 70 }, // 15-18
                    new VehicleRoutingModel.BookingModel { Title = "19", Location = new VehicleRoutingModel.Location { Latitude = -43.55854810, Longitude = 172.5146758 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(8), ServiceMins = 10 }, // 16-18
                    new VehicleRoutingModel.BookingModel { Title = "20", Location = new VehicleRoutingModel.Location { Latitude = -43.54534620, Longitude = 172.6448526 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(8), ServiceMins = 60 }, // 16-18
                    new VehicleRoutingModel.BookingModel { Title = "21", Location = new VehicleRoutingModel.Location { Latitude = -43.53825370, Longitude = 172.6314272 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(4), ServiceToTime = initialDateTime.AddHours(8), ServiceMins = 10 }, // 13-18
                    new VehicleRoutingModel.BookingModel { Title = "22", Location = new VehicleRoutingModel.Location { Latitude = -43.53526933, Longitude = 172.2954649 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(4), ServiceToTime = initialDateTime.AddHours(12), ServiceMins = 60 }, //08-20
                    new VehicleRoutingModel.BookingModel { Title = "23", Location = new VehicleRoutingModel.Location { Latitude = -43.53446700, Longitude = 172.5393696 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(12), ServiceMins = 10 }, // 18-20 
                    new VehicleRoutingModel.BookingModel { Title = "24", Location = new VehicleRoutingModel.Location { Latitude = -43.51218440, Longitude = 172.5027829 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(12), ServiceMins = 5 }, // 19-20
                    new VehicleRoutingModel.BookingModel { Title = "25", Location = new VehicleRoutingModel.Location { Latitude = -43.50909426, Longitude = 172.3271377 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(2), ServiceToTime = initialDateTime.AddHours(12), ServiceMins = 40 }, // 18-20
                    new VehicleRoutingModel.BookingModel { Title = "26", Location = new VehicleRoutingModel.Location { Latitude = -43.39224610, Longitude = 172.5991597 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(2), ServiceToTime = initialDateTime.AddHours(8), ServiceMins = 15 },  // 17-18
                    new VehicleRoutingModel.BookingModel { Title = "27", Location = new VehicleRoutingModel.Location { Latitude = -43.33475818, Longitude = 172.6012256 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime, ServiceToTime = initialDateTime.AddHours(12), ServiceMins = 15 },  // 17-18
                    new VehicleRoutingModel.BookingModel { Title = "28", Location = new VehicleRoutingModel.Location { Latitude = -43.54558020, Longitude = 172.5723066 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(3), ServiceToTime = initialDateTime.AddHours(11), ServiceMins = 15 },  // 17-18
                    new VehicleRoutingModel.BookingModel { Title = "29", Location = new VehicleRoutingModel.Location { Latitude = -43.32319397, Longitude = 172.6015187 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(3), ServiceToTime = initialDateTime.AddHours(11), ServiceMins = 15 },  // 17-18
                    new VehicleRoutingModel.BookingModel { Title = "30", Location = new VehicleRoutingModel.Location { Latitude = -43.54558020, Longitude = 172.5723066 }, FuelType = FuelType.Petrol, ServiceFromTime = initialDateTime.AddHours(3), ServiceToTime = initialDateTime.AddHours(11), ServiceMins = 15 },  // 17-18
                },
                Depot = new VehicleRoutingModel.DepotModel
                {
                    Vehicles = new[]
                    {
                        new VehicleRoutingModel.DepotModel.Vehicle
                        {
                            Name =  "Vehicle 1",
                            DeliveryStartTime = initialDateTime.TimeOfDay,
                            DeliveryEndTime = initialDateTime.AddHours(12).TimeOfDay,
                            FuelType = FuelType.Diesel | FuelType.Petrol
                        },
                        new VehicleRoutingModel.DepotModel.Vehicle
                        {
                            Name = "Vehicle 2",
                            DeliveryStartTime = initialDateTime.TimeOfDay,
                            DeliveryEndTime = initialDateTime.AddHours(12).TimeOfDay,
                            FuelType = FuelType.Diesel | FuelType.Petrol
                        },
                        new VehicleRoutingModel.DepotModel.Vehicle
                        {
                            Name = "Vehicle 3",
                            DeliveryStartTime = initialDateTime.TimeOfDay,
                            DeliveryEndTime = initialDateTime.AddHours(12).TimeOfDay,
                            FuelType = FuelType.Diesel | FuelType.Petrol
                        }
                    },
                    Location = new VehicleRoutingModel.Location
                    {
                        Latitude = -43.55858710975329,
                        Longitude = 172.51475521406968
                    }
                }
            };

            var timeMatrix = await _hereMapsClient.GetHereMapsRoutingMatrixResultAsync(dataset);
            GetTimeWindowsAndServiceTimeMatrix(dataset, out var timeWindows, out var serviceTimeMatrix);

            if (timeMatrix != null && timeMatrix.GetLength(1) == dataset.Bookings.Length + 1)
            {
                dataset = SetTimeMatrixAndDurationForEachLocation(dataset, timeMatrix);

                var manager = new RoutingIndexManager(timeMatrix.GetLength(0), dataset.Depot.Vehicles.Length, 0);
                var routing = new RoutingModel(manager);

                int transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
                {
                    var fromNode = manager.IndexToNode(fromIndex);
                    var toNode = manager.IndexToNode(toIndex);
                    return timeMatrix[fromNode, toNode] + serviceTimeMatrix[fromNode];
                });

                var solver = routing.solver();

                routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);
                routing.AddDimension(transitCallbackIndex, timeWindows[0, 0], timeWindows[0, 1], false, "Time");

                var timeDimension = routing.GetMutableDimension("Time");
                for (int i = 1; i < timeWindows.GetLength(0); ++i)
                {
                    var index = manager.NodeToIndex(i);
                    timeDimension.CumulVar(index).SetRange(timeWindows[i, 0], timeWindows[i, 1]);
                }

                for (int i = 0; i < dataset.Depot.Vehicles.Length; ++i)
                {
                    var index = routing.Start(i);
                    timeDimension.CumulVar(index).SetRange(timeWindows[0, 0], timeWindows[0, 1]);
                }

                for (int i = 0; i < dataset.Depot.Vehicles.Length; ++i)
                {
                    routing.AddVariableMinimizedByFinalizer(timeDimension.CumulVar(routing.Start(i)));
                    routing.AddVariableMinimizedByFinalizer(timeDimension.CumulVar(routing.End(i)));
                }

                RoutingSearchParameters searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
                searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;

                Assignment solution = routing.SolveWithParameters(searchParameters);
                //dataset.TotalDuration = solution.ObjectiveValue();

                if (solution != null)
                {
                    dataset = SetFoundSolution(dataset, routing, manager, solution);
                }
            }

            return dataset;
        }

        [HttpGet, Route("api/datasets/test-vrptw/{testDatasetType:int}")]
        public async Task<VehicleRoutingModel> RunVRPTWTestDataset(TestDatasetType testDatasetType)
        {
            TestDatasetModel testDataset = new TestDatasetModel(testDatasetType);

            var bookings = (
                from data in testDataset.datasets
                select new VehicleRoutingModel.BookingModel
                {
                    FuelType = (FuelType)data.FuelType,
                    ServiceFromTime = data.FromTime,
                    ServiceToTime = data.ToTime,
                    Title = data.Id.ToString(),
                    ServiceMins = data.ServiceTime,
                    Location = new VehicleRoutingModel.Location
                    {
                        Latitude = data.Latitude,
                        Longitude = data.Longitude
                    }
                }
            ).ToArray();

            var initialOperatingStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);
            var dataset = new VehicleRoutingModel
            {
                Bookings = bookings,
                Depot = new VehicleRoutingModel.DepotModel
                {
                    Vehicles = new VehicleRoutingModel.DepotModel.Vehicle[]
                    {
                        new VehicleRoutingModel.DepotModel.Vehicle
                        {
                            Name = "Vehicle 1",
                            FuelType = FuelType.Petrol | FuelType.Diesel,
                            DeliveryStartTime = initialOperatingStartTime.TimeOfDay,
                            DeliveryEndTime = initialOperatingStartTime.AddHours(12).TimeOfDay
                        }
                    },
                    Location = new VehicleRoutingModel.Location
                    {
                        Latitude = testDataset.datasets.First().Latitude,
                        Longitude = testDataset.datasets.First().Longitude
                    }
                }
            };

            var timeMatrix = await _hereMapsClient.GetHereMapsRoutingMatrixResultAsync(dataset);
            GetTimeWindowsAndServiceTimeMatrix(dataset, out var timeWindows, out var serviceTimeMatrix);

            if (timeMatrix != null && timeMatrix.GetLength(1) == dataset.Bookings.Length + 1)
            {
                dataset = SetTimeMatrixAndDurationForEachLocation(dataset, timeMatrix);

                var manager = new RoutingIndexManager(timeMatrix.GetLength(0), dataset.Depot.Vehicles.Length, 0);
                var routing = new RoutingModel(manager);

                int transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
                {
                    var fromNode = manager.IndexToNode(fromIndex);
                    var toNode = manager.IndexToNode(toIndex);
                    return timeMatrix[fromNode, toNode] + serviceTimeMatrix[fromNode];
                });
            }

            return dataset;
        }
    }
}