using System;

namespace CVRPTW
{
    public enum FuelType 
    {
        Petrol = 0,
        Diesel = 1
    }


    public class DataModel
    {
        public Booking[] Bookings { get; set; }

        public class Booking
        {
            public Location Location { get; set; }
            public TimeSpan ServiceFromTime { get; set; }
            public TimeSpan ServiceToTime { get; set; }
            public FuelType[] FuelTypes { get; set; }
            public string Title { get; set; }
        }

        public class Depot 
        {
            public Location Location { get; set; }
            public Vehicle[] Vehicles { get; set; }

            public class Vehicle
            {
                public Location CurrentLocation { get; set; }
                public Container[] Containers { get; set; }

                public class Container
                {
                    public FuelType FuelType { get; set; }
                    public decimal Capacity { get; set; }
                }
            }
        }

        public class Location
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
