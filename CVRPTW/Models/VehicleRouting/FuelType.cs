using System;

namespace CVRPTW.Models.VehicleRouting
{
    [Flags]
    public enum FuelType
    {
        Petrol = 1,
        Diesel = 2
    }

    public static class FuelTypesExtensions
    {
        public static string ToString(this FuelType fueltype)
        {
            switch (fueltype)
            {
                case FuelType.Petrol: return "Petrol";
                case FuelType.Diesel: return "Diesel";
                default: throw new InvalidCastException();
            }
        }
    }
}
