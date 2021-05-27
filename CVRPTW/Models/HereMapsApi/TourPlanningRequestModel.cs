using Newtonsoft.Json;

namespace CVRPTW.Models.HereMapsApi
{
    public class TourPlanningRequestModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("plan")]
        public PlanModel Plan { get; set; }

        [JsonProperty("fleet")]
        public FleetModel Fleet { get; set; }

        [JsonProperty("configuration")]
        public ConfigurationModel Configuration { get; set; }

        public class PlanModel
        {
            [JsonProperty("jobs")]
            public JobModel[] Jobs { get; set; }

            // [JsonProperty("relations")]
            // public RelationModel Relations { get; set; }

            public class JobModel
            {
                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("places")]
                public PlaceModel Places { get; set; }

                [JsonProperty("skills")]
                public string[] Skills { get; set; }

                public class PlaceModel
                {
                    [JsonProperty("deliveries")]
                    public DeliveryModel[] Deliveries { get; set; }
                    public class DeliveryModel
                    {
                        [JsonProperty("times")]
                        public string[][] Times { get; set; }

                        [JsonProperty("location")]
                        public LocationModel Location { get; set; }

                        [JsonProperty("duration")]
                        public int Duration { get; set; }

                        [JsonProperty("demand")]
                        public int[] Demand { get; set; }

                        // [JsonProperty("tag")]
                        // public string Tag { get; set; }


                    }
                }
            }

            public class RelationModel
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("jobs")]
                public string[] Jobs { get; set; }

                [JsonProperty("vehicleId")]
                public string VehicleId { get; set; }
            }
        }

        public class FleetModel
        {
            [JsonProperty("types")]
            public TypeModel[] Types { get; set; }

            [JsonProperty("profiles")]
            public CarProfileModel[] Profiles { get; set; }

            public class TypeModel
            {
                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("profile")]
                public string Profile { get; set; }

                [JsonProperty("costs")]
                public CostsModel Costs { get; set; }

                [JsonProperty("shifts")]
                public VehicleShiftModel[] Shifts { get; set; }

                [JsonProperty("capacity")]
                public int[] Capacity { get; set; }

                [JsonProperty("skills")]
                public string[] Skills { get; set; }

                // [JsonProperty("limits")]
                // public VehicleLimitsModel Limits { get; set; }

                [JsonProperty("amount")]
                public int Amount { get; set; }

                public class CostsModel
                {
                    [JsonProperty("fixed")]
                    public double Fixed { get; set; }

                    [JsonProperty("distance")]
                    public double Distance { get; set; }

                    [JsonProperty("time")]
                    public double Time { get; set; }
                }

                public class VehicleShiftModel
                {
                    [JsonProperty("start")]
                    public ShiftModel Start { get; set; }

                    [JsonProperty("end")]
                    public ShiftModel End { get; set; }

                    // [JsonProperty("breaks")]
                    // public BreakModel[] Breaks { get; set; }

                    public class ShiftModel
                    {
                        [JsonProperty("time")]
                        public string Time { get; set; }

                        [JsonProperty("location")]
                        public LocationModel Location { get; set; }
                    }

                    public class BreakModel
                    {
                        [JsonProperty("times")]
                        public string[] Times { get; set; }

                        [JsonProperty("duration")]
                        public int Duration { get; set; }

                        [JsonProperty("location")]
                        public LocationModel Location { get; set; }
                    }
                }

                public class VehicleLimitsModel
                {
                    [JsonProperty("maxDistance")]
                    public double MaxDistance { get; set; }

                    [JsonProperty("shiftTime")]
                    public double ShiftTime { get; set; }
                }
            }

            public class CarProfileModel
            {
                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("departureTime")]
                public string DepartureTime { get; set; }

                // [JsonProperty("avoid")]
                // public AvoidRouteModel Avoid { get; set; }

                // [JsonProperty("options")]
                // public TruckOptionModel Options { get; set; }

                [JsonProperty("type")]
                public string Type => "truck";

                public class AvoidRouteModel
                {
                    [JsonProperty("features")]
                    public string[] Features { get; set; } //"tollRoad" "motorway" "ferry" "tunnel" "dirtRoad"
                }

                public class TruckOptionModel
                {
                    [JsonProperty("shippedHazardousGoods")]
                    public string[] ShippedHazardousGoods { get; set; } //"explosive" "gas" "flammable" "combustible" "organic" "poison" "radioactive" "corrosive" "poisonousInhalation" "harmfulToWater" "other"

                    [JsonProperty("grossWeight")]
                    public int GrossWeight { get; set; }

                    [JsonProperty("weightPerAxle")]
                    public int WeightPerAxle { get; set; }

                    [JsonProperty("height")]
                    public int Height { get; set; }

                    [JsonProperty("length")]
                    public int Length { get; set; }

                    [JsonProperty("tunnelCategory")]
                    public string TunnelCategory { get; set; } //"B" "C" "D" "E"
                }
            }
        }

        public class ConfigurationModel
        {
            [JsonProperty("optimizations")]
            public OptimizationsModel Optimizations { get; set; }
            public class OptimizationsModel
            {
                [JsonProperty("traffic")]
                public string Traffic { get; set; } // "liveOrHistorical" "historicalOnly" "automatic"

                [JsonProperty("waitingTime")]
                public WaitingTimeModel WaitingTime { get; set; }

                public class WaitingTimeModel
                {
                    [JsonProperty("reduce")]
                    public bool Reduce { get; set; }

                    [JsonProperty("bufferTime")]
                    public int BufferTime { get; set; }
                }

            }
        }
        public class LocationModel
        {
            [JsonProperty("lat")]
            public double Latitude { get; set; }

            [JsonProperty("lng")]
            public double Longitude { get; set; }
        }
    }
}