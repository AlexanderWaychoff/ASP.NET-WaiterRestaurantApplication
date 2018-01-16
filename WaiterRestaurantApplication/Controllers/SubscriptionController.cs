using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WaiterRestaurantApplication.Models;

namespace WaiterRestaurantApplication.Controllers
{
    public class SubscriptionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subscription
        public ActionResult Index(int restaurantId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("RestaurantManager"))
            {
                return HttpNotFound();
            }

            var restaurant = db.Restaurants
                .Include(r => r.Subscription)
                .Include(r => r.Subscription.SubscriptionType)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();

            return View(restaurant);
        }
    }
}