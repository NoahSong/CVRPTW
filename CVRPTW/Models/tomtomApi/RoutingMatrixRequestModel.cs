using Newtonsoft.Json;
using System.Collections.Generic;

namespace CVRPTW.Models.tomtomApi
{
    public class RoutingMatrixRequestModel
    {
        [JsonProperty("origins")]
        public List<Origin> origins { get; set; }
        [JsonProperty("destinations")]
        public List<Destination> destinations { get; set; }
        [JsonProperty("options")]
        public Options options { get; set; }

        public class Point
        {
            [JsonProperty("latitude")]
            public double latitude { get; set; }
            [JsonProperty("longitude")]
            public double longitude { get; set; }
        }
        public class Origin
        {
            [JsonProperty("point")]
            public Point point { get; set; }
        }
        public class Destination
        {
            [JsonProperty("point")]
            public Point point { get; set; }
        }
        public class Post
        {
            [JsonProperty("avoidVignette")]
            public List<string> avoidVignette { get; set; }
        }
        public class Options
        {
            [JsonProperty("post")]
            public Post post { get; set; }
        }
    }
}
