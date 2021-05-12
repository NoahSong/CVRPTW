using System;

namespace CVRPTW.Models.EsriApi
{
    public class RoutingMatrixResponseModel
    {
        public string MatrixId { get; set; }
        public MatrixModel Matrix { get; set; }
        public RegionModel RegionDefinition { get; set; }
        public class MatrixModel
        {
            public int NumOrigins { get; set; }
            public int NumDestinations { get; set; }
            public int[] TravelTimes { get; set; }
            public int[] Distances { get; set; }
        }

        public class RegionModel
        {
            public string Type { get; set; }
            public RegionLocationModel Center { get; set; }
            public int Radius { get; set; }

            public class RegionLocationModel
            {
                public double Lat { get; set; }
                public double Lng { get; set; }
            }
        }
    }
}
