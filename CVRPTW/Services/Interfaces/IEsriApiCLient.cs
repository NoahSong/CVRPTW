using System.Threading.Tasks;
using CVRPTW.Models;
using CVRPTW.Models.VehicleRouting;

namespace CVRPTW.Services.Interfaces
{
    public interface IEsriApiClient
    {
        Task<int[,]> GetEsriMapsRoutingMatrixResultAsync(VehicleRoutingModel parameter);
    }
}
