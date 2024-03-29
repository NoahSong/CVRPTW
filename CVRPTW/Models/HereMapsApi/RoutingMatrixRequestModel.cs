using Newtonsoft.Json;

namespace CVRPTW.Models.HereMapsApi
{
    public class RoutingMatrixRequestModel
    {
        [JsonProperty("origins")]
        public Coordinates[] Origins { get; set; }

        [JsonProperty("regionDefinition")]
        public RegionModel RegionDefinition { get; set; }

        [JsonProperty("routingMode")]
        public string RoutingMode => "short";

        [JsonProperty("matrixAttributes")]
        public string[] MatrixAttributes => new[] { "travelTimes", "distances" };

        public class Coordinates
        {
            [JsonProperty("lat")]
            public double Latitude { get; set; }

            [JsonProperty("lng")]
            public double Longitude { get; set; }
        }

        public class RegionModel
        {
            [JsonProperty("type")]
            public string Type => "autoCircle";
        }
    }
}
