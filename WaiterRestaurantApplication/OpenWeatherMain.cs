using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication
{
    public class OpenWeatherMain
    {
        public string temp { get; set; }
        public string pressure { get; set; }
        public string humidity { get; set; }
        public string temp_min { get; set; }
        public string temp_max { get; set; }

        public OpenWeatherMain(string json)
        {
            JObject jObject = JObject.Parse(json);
            JToken jMain = jObject["main"];
            temp = (string)jMain["temp"];
            pressure = (string)jMain["pressure"];
            humidity = (string)jMain["humidity"];
            temp_min = (string)jMain["temp_min"];
            temp_max = (string)jMain["temp_max"];
        }
    }
}