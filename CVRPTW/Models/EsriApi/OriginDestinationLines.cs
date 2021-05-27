using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVRPTW.Models.EsriApi
{
    public class OriginDestinationLines
    {
        public string paramName { get; set; }
        public string dataType { get; set; }
        public Value value { get; set; }

        public class SpatialReference
        {
            public int wkid { get; set; }
            public int latestWkid { get; set; }
        }

        public class Field
        {
            public string name { get; set; }
            public string type { get; set; }
            public string alias { get; set; }
            public int? length { get; set; }
        }

        public class Attributes
        {
            public int ObjectID { get; set; }
            public int DestinationRank { get; set; }
            public double Total_Time { get; set; }
            public double Total_Distance { get; set; }
            public int OriginOID { get; set; }
            public string OriginName { get; set; }
            public int DestinationOID { get; set; }
            public string DestinationName { get; set; }
            public int Shape_Length { get; set; }
        }

        public class Feature
        {
            public Attributes attributes { get; set; }
        }

        public class Value
        {
            public string displayFieldName { get; set; }
            public string geometryType { get; set; }
            public SpatialReference spatialReference { get; set; }
            public List<Field> fields { get; set; }
            public List<Feature> features { get; set; }
            public bool exceededTransferLimit { get; set; }
        }
    }
}
