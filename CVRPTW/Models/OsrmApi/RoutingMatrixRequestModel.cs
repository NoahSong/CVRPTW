using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVRPTW.Models.OsrmApi
{
    public class RoutingMatrixRequestModel
    {
        public RoutingMatrixRequestModel() {
            Origins = new List<string>();
        }
        // [lat,lng,lat,lng]
        [JsonProperty("origins")]
        public List<string> Origins { get; set; }

        // exclude classes from osm {class}[,{class}]
        [JsonProperty("exclude")]
        public List<string> exclude { get; set; }


        [JsonProperty("matrixAttributes")]
        public string[] MatrixAttributes => new[] { "travelTimes", "distances" };

    }
}
