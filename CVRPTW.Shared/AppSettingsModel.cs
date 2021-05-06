using System;

namespace CVRPTW.Shared
{
    public class AppSettingsModel
    {
        public AppSettingsModel()
        {
            Google = new GoogleModel();
            HereMaps = new HereMapsModel();
        }

        public GoogleModel Google { get; set; }
        public HereMapsModel HereMaps { get; set; }
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
}