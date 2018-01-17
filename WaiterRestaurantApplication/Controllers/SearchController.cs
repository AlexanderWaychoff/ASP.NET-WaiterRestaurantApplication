using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WaiterRestaurantApplication.Models;

namespace WaiterRestaurantApplication.Controllers
{
    public class SearchController : Controller
    {
        private WeatherCondition todaysWeatherCondition = new WeatherCondition("Milwaukee");
        private DateTime todaysDateInformation = DateTime.Now;
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Search
        public ActionResult Index()
        {
            var restaurants = db.Restaurants
                .Include(r => r.Address)
                .Include(r => r.WaitRate)
                .Where(r => r.IsOpen == true)
                .ToList();

            foreach (Restaurant restaurant in restaurants)
            {
                restaurant.EstimatedWaitTime = CalculateEstimatedWaitTimes(restaurant.RestaurantId);
            }

            ViewBag.StateId = new SelectList(db.States, "StateId", "Abbreviation");

            return View(restaurants);
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

            int estimatedWaitTime = -1;
            if ( tableVisits.Count>0 )
            {
                estimatedWaitTime = totalMinutes / tableVisits.Count;
            }

            return estimatedWaitTime;

        }

    }
}