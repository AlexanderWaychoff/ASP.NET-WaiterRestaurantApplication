using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication.Models
{
    public class WeatherCondition
    {
        public int WeatherConditionId { get; set; }
        public int Temperature { get; set; }
        public string WeatherDescription { get; set; }

        public WeatherCondition()
        {
            string json = WeatherJSONReader.GetJsonString();
            JObject jObject = JObject.Parse(json);
            JToken jWeather = jObject["weather"];
            JToken jMain = jObject["main"];
            string temp = (string)jMain["temp"];
            Temperature = WeatherJSONReader.ConvertToFahrenheit(temp);
            WeatherDescription = (string)jWeather[0]["description"];
        }
    }
}