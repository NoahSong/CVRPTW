using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace CVRPTW.Datasets
{
    public enum TestDatasetType
    {
        With5Data =5,
        With10Data = 10,
        With100Data = 100,
        with1000Data = 1000
    }
    public class TestDatasetModel
    {
        TestDatasetType _testDatasetType;
        public DatasetModel[] datasets { get; set; }
        public TestDatasetModel(TestDatasetType testDatasetType)
        {
            _testDatasetType = testDatasetType;

            InitialiseDataset();
        }

        private void InitialiseDataset()
        {
            string filePath = string.Empty;
            var path = AppDomain.CurrentDomain.BaseDirectory;

            switch (_testDatasetType)
            {
                case TestDatasetType.With5Data: filePath = path + "testDataset5.json"; break;
                case TestDatasetType.With10Data: filePath = path + "testDataset10.json"; break;
                case TestDatasetType.With100Data: filePath = path + "testDataset100.json"; break;
                case TestDatasetType.with1000Data: filePath = path + "testDataset1000.json"; break;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string json = reader.ReadToEnd();
                datasets = JsonConvert.DeserializeObject<DatasetModel[]>(json);
            }
        }

        public class DatasetModel
        {
            [JsonProperty("ID")]
            public int Id { get; set; }

            [JsonProperty("from_")]
            public DateTime FromTime { get; set; }

            [JsonProperty("to_")]
            public DateTime ToTime { get; set; }

            [JsonProperty("service_ti")]
            public int ServiceTime { get; set; }

            [JsonProperty("fueltype")]
            public int FuelType { get; set; }

            [JsonProperty("lat")]
            public double Latitude { get; set; }

            [JsonProperty("lng")]
            public double Longitude { get; set; }
        }
    }
}
