using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace WaiterRestaurantApplication
{
    public static class GoogleMapAPI
    {
        static WebClient client = new WebClient();
        static string Json = client.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=milwaukee&appid=5779a8c61a125dd73ee705c06d68a347");
        static JavaScriptSerializer s1 = new JavaScriptSerializer();
        public static string GetJsonString(string cityName)
        {
            try
            {
                string Json = client.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&appid=5779a8c61a125dd73ee705c06d68a347");
                return Json;
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }
    }
}