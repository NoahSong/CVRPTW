using System.Threading.Tasks;
using CVRPTW.Models.tomtomApi;
using CVRPTW.Models.VehicleRouting;

namespace CVRPTW.Services
{
    public interface ITomtomMapsApiClient
    {
        Task<int[,]> GetTomtomRoutingMatrixResultAsync(VehicleRoutingModel parameter);
    }
}