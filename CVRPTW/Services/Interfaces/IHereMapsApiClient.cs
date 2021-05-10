using System.Threading.Tasks;
using CVRPTW.Models;
using CVRPTW.Models.VehicleRouting;

namespace CVRPTW.Services
{
    public interface IHereMapsApiClient
    {
        Task<RoutingMatrixResultModel> GetHereMapsRoutingMatrixResultAsync(VehicleRoutingModel parameter);
    }
}
