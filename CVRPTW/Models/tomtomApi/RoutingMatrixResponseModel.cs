using System;
using System.Collections.Generic;

namespace CVRPTW.Models.tomtomApi
{
    public class RoutingMatrixResponseModel
    {

        public class Summary
        {
            public int successfulRoutes { get; set; }
            public int totalRoutes { get; set; }
        }
        public class RouteSummary
        {
            public int lengthInMeters { get; set; }
            public int travelTimeInSeconds { get; set; }
            public int trafficDelayInSeconds { get; set; }
            public int trafficLengthInMeters { get; set; }
            public DateTime departureTime { get; set; }
            public DateTime arrivalTime { get; set; }
        }

        public class Response
        {
            public RouteSummary routeSummary { get; set; }
        }

        public class Root
        {
            public int statusCode { get; set; }
            public Response response { get; set; }
        }
        public string formatVersion { get; set; }
        public List<List<Root>> matrix { get; set; }
        public Summary summary { get; set; }

    }
}
