using System;

namespace CVRPTW.Models.HereMapsApi
{
    public class RoutingMatrixResultModel
    {
        public int[,] Matrix { get; set; }
        public LocationModel Center { get; set; }
        public int Radius { get; set; }

        public class LocationModel
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }
    }
}
