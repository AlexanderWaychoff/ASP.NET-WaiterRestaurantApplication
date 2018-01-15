using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication
{
    public class OpenWeatherWeather
    {
        public string id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }

        public OpenWeatherWeather(string json)
        {
            JObject jObject = JObject.Parse(json);
            JToken jWeather = jObject["weather"];
            id = (string)jWeather[0]["id"];
            main = (string)jWeather[0]["main"];
            description = (string)jWeather[0]["description"];
            icon = (string)jWeather[0]["icon"];
        }
    }
}