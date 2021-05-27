using System;

namespace CVRPTW.Shared
{
    public class AppSettingsModel
    {
        public AppSettingsModel()
        {
            Google = new GoogleModel();
            HereMaps = new HereMapsModel();
            Osrm = new OSRMModel();
            Tomtom = new TomtomModel();
            Esri = new EsriModel();
        }

        public GoogleModel Google { get; set; }
        public HereMapsModel HereMaps { get; set; }

        public EsriModel Esri { get; set; }
        public OSRMModel Osrm { get; set; }
        public TomtomModel Tomtom { get; set; }
    }

    public class GoogleModel
    {
        public MapsModel Maps { get; set; }
        public class MapsModel
        {
            public string ApiKey { get; set; }
        }
    }

    public class HereMapsModel
    {
        public string RoutingMatrixUrl { get; set; }
        public string TourPlanningUrl { get; set; }
        public string ApiKey { get; set; }
    }

    public class EsriModel
    {
        public string RoutingMatrixUrl { get; set; }
        public string ApiKey { get; set; }

        public string JobStatusUrl { get; set; }
    }

    public class OSRMModel
    {
        public string RoutingMatrixUrl { get; set; }
    }
    public class TomtomModel
    {
        public string RoutingMatrixUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
