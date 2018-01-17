using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WaiterRestaurantApplication.Models;

namespace WaiterRestaurantApplication.Controllers
{
    public class AnalyticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private WeatherCondition todaysWeatherCondition = new WeatherCondition("Milwaukee");
        private DateTime todaysDateInformation = DateTime.Now;

        public void CalculateWaitRate(TableVisit currentTableVisit)
        {
            Restaurant currentRestaurant = db.Restaurants.Find(currentTableVisit.RestaurantId);
            int totalRatings = 0;
            int yesTotal = 0;

            foreach(var customer in db.TableVisits)
            {
                if(customer.RestaurantId == currentRestaurant.RestaurantId)
                {
                    totalRatings++;
                    if(customer.IsSatisfied == true)
                    {
                        yesTotal++;
                    }
                }
            }

            int WaitRate = (yesTotal / totalRatings)*100;

            WaitRate rate = db.WaitRate.Find(currentRestaurant.WaitRateId);
            rate.WaitRatePercentage = WaitRate;
        }

        public int CalculateEstimatedWaitTimes(int restaurantId)
        {
            int temperatureRangeToTest = 30;

            var tableVisits = db.TableVisits
                .Include(r => r.WeatherCondition)
                .Where(r => r.RestaurantId == restaurantId)
                .Where(r => r.WeatherCondition.Temperature <= todaysWeatherCondition.Temperature + temperatureRangeToTest && r.WeatherCondition.Temperature >= todaysWeatherCondition.Temperature - temperatureRangeToTest)
                .ToList();

            tableVisits = tableVisits.Where(r => r.CreatedOn.DayOfWeek.Equals(todaysDateInformation.DayOfWeek)).ToList();

            int totalMinutes = 0;
            foreach (TableVisit tableVisit in tableVisits)
            {
                    totalMinutes += tableVisit.WaitMinutes;
            }

            int estimatedWaitTime = totalMinutes / tableVisits.Count;

            return estimatedWaitTime;


        }
        // GET: Analytics
        public ActionResult Index()
        {
            return View();
        }
    }
}