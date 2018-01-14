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
    public class RestaurantController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Restaurant
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!(User.IsInRole("RestaurantManager") || User.IsInRole("RestaurantEmployee")))
            {
                return HttpNotFound();
            }
            if ( User.IsInRole("RestaurantEmployee") )
            {
                return RedirectToAction("DisplayMyRestaurants", "Restaurant");
            }

            //User is Restaurant Manager
            var userId = User.Identity.GetUserId();
            var restaurants = db.Restaurants
                .Include(r => r.Address)
                .Where(r => r.UserId == userId)
                .ToList();
            return View(restaurants);
        }

        // GET: Restaurant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // GET: Restaurant/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("RestaurantManager"))
            {
                return HttpNotFound();
            }

            ViewBag.StateId = new SelectList(db.States, "StateId", "Abbreviation");
            return View();
        }

        // POST: Restaurant/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "RestaurantId,RestaurantName,AddressId,OpenTime,CloseTime,IsOpen,PeopleBeforeWarning,GracePeriodMinutes,CurrentWaitMinutes")] Restaurant restaurant)
        public ActionResult Create(string RestaurantName, string streetOne, string streetTwo, string city, string StateId, string ZipCode_Number, string lat, string lng)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("RestaurantManager"))
            {
                return HttpNotFound();
            }


            Restaurant restaurant = new Restaurant();
            if (ModelState.IsValid)
            {
                Address address = GetAddress(streetOne, city, StateId, ZipCode_Number, lat, lng);
                restaurant.RestaurantName = RestaurantName;
                restaurant.AddressId = address.AddressId;
                restaurant.UserId = User.Identity.GetUserId();
                db.Restaurants.Add(restaurant);
                db.SaveChanges();
                return RedirectToAction("Index", "Restaurant");
            }

            ViewBag.StateId = new SelectList(db.States, "StateId", "Abbreviation");
            return View(restaurant);
        }

        private Address GetAddress(string StreetOne, string City_Name, string StateId, string ZipCode_Number, string lat, string lng)
        {
            int stateIdNumber = Convert.ToInt32(StateId);
            if (db.Addresses.Any(a => a.StreetOne == StreetOne && a.City.Name == City_Name && a.State.StateId == stateIdNumber && a.ZipCode.Number == ZipCode_Number))
            {
                var addressFound = db.Addresses.First(a => a.StreetOne == StreetOne && a.City.Name == City_Name && a.State.StateId == stateIdNumber && a.ZipCode.Number == ZipCode_Number);
                return new Address();
            }
            Address address = new Address();
            address.StreetOne = StreetOne;
            address.CityId = GetCityID(City_Name);
            address.StateId = GetStateID(StateId);
            address.ZipCodeId = GetZipCodeID(ZipCode_Number);
            address.Lat = Convert.ToSingle(lat);
            address.Lng = Convert.ToSingle(lng);
            db.Addresses.Add(address);
            db.SaveChanges();

            return address;
        }

        private int GetStateID(string StateId)
        {
            int stateIdNumber = Convert.ToInt32(StateId);
            var stateFound = db.States.First(s => s.StateId == stateIdNumber);
            return stateFound.StateId;
        }

        private int GetZipCodeID(string ZipCode_Number)
        {
            if (db.ZipCodes.Any(z => z.Number == ZipCode_Number))
            {
                var zipCodeFound = db.ZipCodes.First(z => z.Number == ZipCode_Number);
                return zipCodeFound.ZipCodeId;
            }
            ZipCode zipCode = new ZipCode();
            zipCode.Number = ZipCode_Number;
            db.ZipCodes.Add(zipCode);
            db.SaveChanges();
            return zipCode.ZipCodeId;
        }

        private int GetCityID(string City_Name)
        {
            if (db.Cities.Any(c => c.Name.ToLower() == City_Name.ToLower()))
            {
                var cityFound = db.Cities.First(c => c.Name == City_Name);
                return cityFound.CityId;
            }
            City city = new City();
            string formattedName = char.ToUpper(City_Name[0]) + City_Name.Substring(1).ToLower();
            city.Name = formattedName;
            db.Cities.Add(city);
            db.SaveChanges();
            return city.CityId;
        }

        // GET: Restaurant/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressId = new SelectList(db.Addresses, "AddressId", "StreetOne", restaurant.AddressId);
            return View(restaurant);
        }

        // POST: Restaurant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RestaurantId,RestaurantName,AddressId,OpenTime,CloseTime,IsOpen,PeopleBeforeWarning,GracePeriodMinutes,CurrentWaitMinutes")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(restaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddressId = new SelectList(db.Addresses, "AddressId", "StreetOne", restaurant.AddressId);
            return View(restaurant);
        }

        // GET: Restaurant/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            db.Restaurants.Remove(restaurant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult DisplayMyRestaurants()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("RestaurantEmployee"))
            {
                return HttpNotFound();
            }

            //User is Restaurant Employee
            //Get all restaurants
            var allRestaurants = db.Restaurants
                .Include(r => r.Employees)
                .ToList();
            List<Restaurant> myRestaurants = new List<Restaurant>();
            string userId = User.Identity.GetUserId();
            var user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            foreach (Restaurant restaurant in allRestaurants)
            {
                if ( restaurant.Employees != null )
                {
                    if (restaurant.Employees.Contains(user))
                    {
                        myRestaurants.Add(restaurant);
                    }
                }
            }
            return View(myRestaurants);
        }

        public ActionResult ConfirmEmployment()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("RestaurantEmployee"))
            {
                return HttpNotFound();
            }

            //User is Restaurant Employee
            var userId = User.Identity.GetUserId();
            var restaurants = db.Restaurants
                .Include(r => r.Address)
                .ToList();
            return View(restaurants);
        }

        [HttpPost]
        public ActionResult ConfirmEmployment(int restaurantId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("RestaurantEmployee"))
            {
                return HttpNotFound();
            }

            var restaurant = db.Restaurants
                .Include(r => r.Employees)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();
            var userId = User.Identity.GetUserId();
            var user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            restaurant.Employees.Add(user);
            db.SaveChanges();
            return RedirectToAction("DisplayMyRestaurants", "Restaurant");
        }

    }
}
