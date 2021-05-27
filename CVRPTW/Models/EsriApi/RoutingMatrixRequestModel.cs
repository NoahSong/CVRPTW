using Newtonsoft.Json;
using System.Collections.Generic;

namespace CVRPTW.Models.EsriApi
{
    public class RoutingMatrixRequestModel
    {
        [JsonProperty("origins")]
        public Features Origins { get; set; }

        [JsonProperty("destinations")]
        public Features Destinations { get; set; }

        [JsonProperty("routingMode")]
        public string RoutingMode => "short";

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("f")]
        public string Format => "json";

        [JsonProperty("travel_mode")]
        public string TravelMode => "Trucking Time";

        [JsonProperty("time_of_day")]
        public string TimeOfDay => "";

        [JsonProperty("Time_Units")]
        public string TimeUnits => "Seconds";

        

        public class Coordinates
        {
            [JsonProperty("y")]
            public double Latitude { get; set; }

            [JsonProperty("x")]
            public double Longitude { get; set; }
        }

        public class Features
        {
            [JsonProperty("spatialReference")]
            public string SpatialReference => "{\"wkid\":102100}";

            [JsonProperty("features")]
            public List<Feature> FeaturesList { get; set; }
        }

        public class Feature
        {
            [JsonProperty("geometry")]
            public Coordinates geometry { get; set; }

            [JsonProperty("attributes")]
            public Attributes Attributes { get; set; }

        }

        public class Attributes
        {
            [JsonProperty("id")]
            public string id { get; set; }


            [JsonProperty("name")]
            public string name { get; set; }

        }

    }
}
