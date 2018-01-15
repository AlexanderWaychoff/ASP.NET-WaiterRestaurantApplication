using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using System.Web.Services.Description;

namespace WaiterRestaurantApplication
{
    public static class WeatherJSONReader
    {
        static WebClient client = new WebClient();
        static string Json = client.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=milwaukee&appid=5779a8c61a125dd73ee705c06d68a347");
        static JavaScriptSerializer s1 = new JavaScriptSerializer();
        public static void Deserialize()
        {
            //JToken weatherJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Json);
            //Weather weather = s1.Deserialize<Weather>(Json);
            OpenWeatherWeather weather = new OpenWeatherWeather(Json);
            OpenWeatherMain main = new OpenWeatherMain(Json);
            return;
        }
        public static string GetJsonString()
        {
            try
            {
                string Json = client.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=milwaukee&appid=5779a8c61a125dd73ee705c06d68a347");
                return Json;
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }
        public static int ConvertToFahrenheit(string temp)
        {
            int temperature = Convert.ToInt32(temp);
            int roundedTemperature = Convert.ToInt32(temperature * 9 / 5 - 459.67);
            return 0;
        }
    }
}