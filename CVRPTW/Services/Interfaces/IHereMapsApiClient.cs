using System.Threading.Tasks;
using CVRPTW.Models.HereMapsApi;
using CVRPTW.Models.VehicleRouting;

namespace CVRPTW.Services
{
    public interface IHereMapsApiClient
    {
        Task<int[,]> GetHereMapsRoutingMatrixResultAsync(VehicleRoutingModel parameter);
    }
}
