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
        }

        public GoogleModel Google { get; set; }
        public HereMapsModel HereMaps { get; set; }
        public OSRMModel Osrm { get; set; }
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
        public string ApiKey { get; set; }
    }

    public class OSRMModel
    {
        public string RoutingMatrixUrl { get; set; }
    }

}
