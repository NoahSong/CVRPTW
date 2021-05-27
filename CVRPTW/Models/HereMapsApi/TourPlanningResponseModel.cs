using Newtonsoft.Json;

namespace CVRPTW.Models.HereMapsApi
{
    public class TourPlanningResponseModel
    {
        [JsonProperty("problemId")]
        public string ProblemId { get; set; }

        [JsonProperty("statistic")]
        public StatisticModel Statistic { get; set; }

        [JsonProperty("tours")]
        public TourModel[] Tours { get; set; }

        [JsonProperty("unassigned")]
        public UnassignedJobModel[] Unassigned { get; set; }

        public class UnassignedJobModel
        {
            [JsonProperty("jobId")]
            public string JobId { get; set; }

            [JsonProperty("reasons")]
            public UnassignedJobReasonModel[] Reasons { get; set; }

            public class UnassignedJobReasonModel
            {
                [JsonProperty("code")]
                public string Code { get; set; }

                [JsonProperty("description")]
                public string Description { get; set; }
            }
        }

        public class StatisticModel
        {
            [JsonProperty("cost")]
            public double Cost { get; set; }

            [JsonProperty("distance")]
            public int Distance { get; set; }

            [JsonProperty("duration")]
            public int Duration { get; set; }

            [JsonProperty("times")]
            public TimingModel Times { get; set; }

            public class TimingModel
            {
                [JsonProperty("driving")]
                public int Driving { get; set; }

                [JsonProperty("serving")]
                public int Serving { get; set; }

                [JsonProperty("waiting")]
                public int Waiting { get; set; }

                [JsonProperty("break")]
                public int Break { get; set; }
            }
        }

        public class TourModel
        {
            [JsonProperty("vehicleId")]
            public string VehicleId { get; set; }

            [JsonProperty("typeId")]
            public string TypeId { get; set; }

            [JsonProperty("stops")]
            public StopModel[] Stops { get; set; }

            [JsonProperty("statistic")]
            public StatisticModel Statistic { get; set; }
            public class StopModel
            {
                [JsonProperty("location")]
                public LocationModel Location { get; set; }

                [JsonProperty("time")]
                public ScheduleModel Time { get; set; }

                [JsonProperty("load")]
                public int[] Load { get; set; }

                [JsonProperty("activities")]
                public ActivityModel[] Activities { get; set; }

                public class ActivityModel
                {
                    [JsonProperty("jobId")]
                    public string JobId { get; set; }

                    [JsonProperty("jobTag")]
                    public string JobTag { get; set; }

                    [JsonProperty("type")]
                    public string Type { get; set; } //"departure" "arrival" "pickup" "delivery" "break"

                    [JsonProperty("location")]
                    public LocationModel Location { get; set; }

                    [JsonProperty("time")]
                    public TimeModel Time { get; set; }

                    public class TimeModel
                    {
                        [JsonProperty("start")]
                        public string Start { get; set; }

                        [JsonProperty("end")]
                        public string End { get; set; }
                    }
                }

                public class ScheduleModel
                {
                    [JsonProperty("arrival")]
                    public string Arrival { get; set; }

                    [JsonProperty("departure")]
                    public string Departure { get; set; }
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