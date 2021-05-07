using Google.OrTools.ConstraintSolver;
using CVRPTW.Models.VehicleRouting;

namespace CVRPTW.Clients.OrTools
{
    public class TimeCallback
    {
        private VehicleRoutingModel _dataset;
        private RoutingIndexManager _manager;
        private int[,] _timeMatrix;
        private int[] _serviceTimeMatrix;
        private FuelType? _vehicleFuelType;

        public LongLongToLong Callback { get; set; }

        public TimeCallback(VehicleRoutingModel dataset, RoutingIndexManager manager, int[,] timeMatrix, int[] serviceTimeMatrix = null, FuelType? fuelType = null)
        {
            _dataset = dataset;
            _timeMatrix = timeMatrix;
            _serviceTimeMatrix = serviceTimeMatrix;
            _manager = manager;
            _vehicleFuelType = fuelType;

            if (serviceTimeMatrix != null && fuelType != null)
            {
                Callback = RunWithServiceTimeAndFuelType;
            }
            else if (serviceTimeMatrix != null && fuelType == null)
            {
                Callback = RunWithServiceTime;
            }
            else
            {
                Callback = Run;
            }
        }

        private long Run(long fromIndex, long toIndex)
        {
            var fromNode = _manager.IndexToNode(fromIndex);
            var toNode = _manager.IndexToNode(toIndex);
            return _timeMatrix[fromNode, toNode];
        }

        private long RunWithServiceTime(long fromIndex, long toIndex)
        {
            var fromNode = _manager.IndexToNode(fromIndex);
            var toNode = _manager.IndexToNode(toIndex);
            return _timeMatrix[fromNode, toNode] + _serviceTimeMatrix[fromNode];
        }

        private long RunWithServiceTimeAndFuelType(long fromIndex, long toIndex)
        {
            //TODO: Calculate with the consideration of fuel type.
            var fromNode = _manager.IndexToNode(fromIndex);
            var toNode = _manager.IndexToNode(toIndex);

            if (fromNode != 0 && _dataset.Bookings[fromNode - 1].FuelType != _vehicleFuelType)
            {
                return -1;
            }
            else
            {
                return _timeMatrix[fromNode, toNode] + _serviceTimeMatrix[fromNode];
            }
        }
    }
}
