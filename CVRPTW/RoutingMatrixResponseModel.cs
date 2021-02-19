using System;
using System.Collections.Generic;
using System.Text;

namespace CVRPTW
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
            public double West { get; set; }
            public double East { get; set; }
            public double North { get; set; }
            public double South { get; set; }
        }

    }
}
