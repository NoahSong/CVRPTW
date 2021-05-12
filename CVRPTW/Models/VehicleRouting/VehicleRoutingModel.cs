using System;
using System.Collections.Generic;

namespace CVRPTW.Models.VehicleRouting
{
    public class VehicleRoutingModel
    {
        public DepotModel Depot { get; set; }
        public BookingModel[] Bookings { get; set; }
        public long TotalDuration { get; set; }

        public Location Center { get; set; }
        public int Radius { get; set; }

        public class BookingModel
        {
            public Location Location { get; set; }
            public DateTime ServiceFromTime { get; set; }
            public DateTime ServiceToTime { get; set; }
            public int ServiceMins { get; set; }
            public FuelType FuelType { get; set; }
            public string Title { get; set; }
            public int[] TimeMatrix { get; set; }
            public List<Point> Points { get; set; }
            public long Order { get; set; }
            public long NextNodeIndex { get; set; }
        }

        public class Point
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public int Duration { get; set; }
        }

        public class DepotModel
        {
            public Location Location { get; set; }
            public Vehicle[] Vehicles { get; set; }

            public class Vehicle
            {
                public string Name { get; set; }
                public DateTime DeliveryStartTime { get; set; }
                public DateTime DeliveryEndTime { get; set; }
                public Location CurrentLocation { get; set; }
                public BookingModel[] OrdinalBookings { get; set; }
                public long TotalDuration { get; set; }
                public FuelType FuelType { get; set; }
            }

            public int[] TimeMatrix { get; set; }
            public List<Point> Points { get; set; }
            public long NextNodeIndex { get; set; }
        }

        public class Location
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
