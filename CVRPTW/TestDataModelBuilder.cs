using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVRPTW
{
    public class TestDataModelBuilder
    {
        private DataModel dataModel;

        public TestDataModelBuilder()
        {
            var initialDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            dataModel = new DataModel
            {
                Bookings = new[]
                {
                    new DataModel.Booking {Title = "Test 1", Location =  new DataModel.Location{ Latitude = -43.5247272079374, Longitude = 172.633513100445}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.TimeOfDay,ServiceToTime = initialDateTime.AddHours(2).TimeOfDay },
                    new DataModel.Booking {Title = "Test 2", Location =  new DataModel.Location{ Latitude = -43.5175961, Longitude = 172.6059798}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(2).TimeOfDay,ServiceToTime = initialDateTime.AddHours(4).TimeOfDay },
                    new DataModel.Booking {Title = "Test 3", Location =  new DataModel.Location{ Latitude = -43.5334978874869, Longitude = 172.640193868459}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(4).TimeOfDay,ServiceToTime = initialDateTime.AddHours(6).TimeOfDay },
                    new DataModel.Booking {Title = "Test 4", Location =  new DataModel.Location{ Latitude = -43.5014896810655, Longitude = 172.538901482488}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(6).TimeOfDay,ServiceToTime = initialDateTime.AddHours(8).TimeOfDay },
                    new DataModel.Booking {Title = "Test 5", Location =  new DataModel.Location{ Latitude = -43.4991287626566, Longitude = 172.559491940761}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 6", Location =  new DataModel.Location{ Latitude = -43.5165243765856, Longitude = 172.57843149594}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 7", Location =  new DataModel.Location{ Latitude = -43.4863798482809, Longitude = 172.551991713127}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 8", Location =  new DataModel.Location{ Latitude = -43.4759860868474, Longitude = 172.554149507561}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 9", Location =  new DataModel.Location{ Latitude = -43.5023704241763,  Longitude = 172.557818220199}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 10", Location =  new DataModel.Location{ Latitude = -43.4843287738531, Longitude = 172.55139390988}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 11", Location =  new DataModel.Location{ Latitude = -43.4985692017492, Longitude = 172.541027833797}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 12", Location =  new DataModel.Location{ Latitude = -43.473895788487 , Longitude = 172.556694173693}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 13", Location =  new DataModel.Location{ Latitude = -43.4724853197637, Longitude = 172.561076669109}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 14", Location =  new DataModel.Location{ Latitude = -43.5026636406551, Longitude = 172.610007690316}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 15", Location =  new DataModel.Location{ Latitude = -43.4744394333356, Longitude = 172.595296517928}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 16", Location =  new DataModel.Location{ Latitude = -43.4914543330767, Longitude = 172.569685423772}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 17", Location =  new DataModel.Location{ Latitude = -43.5186234595116, Longitude = 172.599321894786}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 18", Location =  new DataModel.Location{ Latitude = -43.5173158522958, Longitude = 172.625684953758}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 19", Location =  new DataModel.Location{ Latitude = -43.521171231041,  Longitude = 172.633790586116}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 20", Location =  new DataModel.Location{ Latitude = -43.5185677789851, Longitude = 172.528990250009}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 21", Location =  new DataModel.Location{ Latitude = -43.4904564017631, Longitude = 172.593120672244}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 22", Location =  new DataModel.Location{ Latitude = -43.5262428677891, Longitude = 172.5174277961}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 23", Location =  new DataModel.Location{ Latitude = -43.5093254409664, Longitude = 172.557358430945}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 24", Location =  new DataModel.Location{ Latitude = -43.4693022074228, Longitude = 172.565086262462}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 25", Location =  new DataModel.Location{ Latitude = -43.4842819145973, Longitude = 172.579443554393}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 26", Location =  new DataModel.Location{ Latitude = -43.4998133647305, Longitude = 172.62926050467}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 27", Location =  new DataModel.Location{ Latitude = -43.5066318846579, Longitude = 172.61417482523}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 28", Location =  new DataModel.Location{ Latitude = -43.4916102745955, Longitude = 172.548162057325}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 29", Location =  new DataModel.Location{ Latitude = -43.5237280622046, Longitude = 172.581154893682}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                    new DataModel.Booking {Title = "Test 30", Location =  new DataModel.Location{ Latitude = -43.4949456581424, Longitude = 172.545153760118}, FuelTypes = new [] { FuelType.Petrol }, ServiceFromTime = initialDateTime.AddHours(8).TimeOfDay,ServiceToTime = initialDateTime.AddHours(10).TimeOfDay },
                }
            };
        }

        public DataModel.Location[] GetLocations()
        {
            return dataModel.Bookings.Select(b => b.Location).ToArray();     
        }
    }
}
