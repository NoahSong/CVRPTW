using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVRPTW.Models.EsriApi
{
    public class ApiJobResponse
    {
        [JsonProperty("jobId")]
        public string JobId { get; set; }

        [JsonProperty("jobStatus")]
        public string JobStatus { get; set; }

        [JsonProperty("messages")]
        public List<ApiMessage> Messages { get; set; }

        public class ApiMessage
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }
        }
    }

}

