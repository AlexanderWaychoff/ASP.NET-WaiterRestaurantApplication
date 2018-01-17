using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaiterRestaurantApplication.Models;

namespace WaiterRestaurantApplication.Controllers
{
    public class AnalyticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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

            int WaitRate = Convert.ToInt32((yesTotal / totalRatings)*100);

            currentRestaurant.WaitRate.WaitRatePercentage = WaitRate;
        }
        // GET: Analytics
        public ActionResult Index()
        {
            return View();
        }
    }
}