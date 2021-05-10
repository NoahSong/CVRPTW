using System;
using CVRPTW.Models.VehicleRouting;
using Google.OrTools.ConstraintSolver;

namespace CVRPTW.Clients.OrTools
{
    public class UnaryTransitCallback
    {
        private VehicleRoutingModel _dataset;
        private RoutingIndexManager _manager;
        private int _vehicleCapacity;
        private FuelType _fuelType;

        public LongToLong Callback { get; set; }

        public UnaryTransitCallback(VehicleRoutingModel dataset, RoutingIndexManager manager, int vehicleCapacity, FuelType fuelType)
        {
            _dataset = dataset;
            _vehicleCapacity = vehicleCapacity;
            _fuelType = fuelType;
            _manager = manager;

            Callback = Run;
        }

        private long Run(long fromIndex)
        {
            var fromNode = _manager.IndexToNode(fromIndex);

            if (fromNode == 0)
            {
                return 0;
            }
            else if ((_dataset.Bookings[fromNode - 1].FuelType & _fuelType) >= _dataset.Bookings[fromNode - 1].FuelType)
            {
                return 1;
            }
            else
            {
                return _vehicleCapacity + 1;
            }
        }
    }
}
