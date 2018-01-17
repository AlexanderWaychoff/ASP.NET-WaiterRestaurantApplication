using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WaiterRestaurantApplication.Models;


namespace WaiterRestaurantApplication.Controllers
{
    public class DinerController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Diner
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchRestaurantOptions()
        {
            return View();
        }

        public ActionResult SearchRestaurantsNearDiner()
        {
            //var addresses = db.Addresses
            //.Where(x => x.Lat <= ; compare current location to addresses; 0.1449275362318841 = roughly 10 miles in ltd/lng
            //.Include(x => x.Ad)
            var restaurantInfo = db.Restaurants
                .Include(r => r.Address)
                .Include(r => r.Address.City)
                .Where(r => r.IsOpen == true)
                .FirstOrDefault();

            return View(restaurantInfo);
        }
    }
}