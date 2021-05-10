using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVRPTW.Models.OsrmApi
{
    public class RoutingMatrixResponseModel
    {
        public class Source
        {
            public string hint { get; set; }
            public double distance { get; set; }
            public List<double> location { get; set; }
            public string name { get; set; }
        }

        public class Destination
        {
            public string hint { get; set; }
            public double distance { get; set; }
            public List<double> location { get; set; }
            public string name { get; set; }
        }
        public string code { get; set; }
        public List<List<double>> durations { get; set; }
        public List<Source> sources { get; set; }
        public List<Destination> destinations { get; set; }
    }
}
