using System.Threading.Tasks;
using CVRPTW.Models;
using CVRPTW.Models.HereMapsApi;
using CVRPTW.Models.VehicleRouting;

namespace CVRPTW.Services
{
    public interface IHereMapsApiClient
    {
        Task<RoutingMatrixResultModel> GetHereMapsRoutingMatrixResultAsync(VehicleRoutingModel parameter);
        Task<TourPlanningResponseModel> GetHereMapsTourPlanningResultAsync(VehicleRoutingModel parameter);
    }
}
