using System.Threading.Tasks;
using CVRPTW.Models.OsrmApi;
using CVRPTW.Models.VehicleRouting;
namespace CVRPTW.Services
{
    public interface IOsrmApiClient
    {
        Task<int[,]> GetHereMapsRoutingMatrixResultAsync(VehicleRoutingModel parameter);
    }
}
