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
            //set up a viewModel with both active and inactive restaurants
            var userId = User.Identity.GetUserId();
            var activeRestaurants = db.Restaurants
                .Include(r => r.Address)
                .Include(r => r.PendingEmployees)
                .Include(r => r.ConfirmedEmployees)
                .Include(r => r.Subscription)
                .Where(r => r.UserId == userId)
                .Where(r => r.Subscription.IsActive == true)
                .ToList();

            var inactiveRestaurants = db.Restaurants
                .Include(r => r.Address)
                .Include(r => r.PendingEmployees)
                .Include(r => r.ConfirmedEmployees)
                .Include(r => r.Subscription)
                .Where(r => r.UserId == userId)
                .Where(r => r.Subscription.IsActive == false || r.Subscription == null)
                .ToList();

            RestaurantIndexViewModel viewModel = new RestaurantIndexViewModel();
            viewModel.ActiveRestaurants = activeRestaurants;
            viewModel.InactiveRestaurants = inactiveRestaurants;

            return View(viewModel);
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
        public ActionResult Create(string RestaurantName, string streetOne, string streetTwo, string city, string StateId, string zipCode, string lat, string lng)
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
                Address address = GetAddress(streetOne, city, StateId, zipCode, lat, lng);
                if (address.AddressId == 0)
                {
                    return RedirectToAction("DuplicateAddress", "Restaurant");
                }
                restaurant.RestaurantName = RestaurantName;
                restaurant.AddressId = address.AddressId;
                restaurant.UserId = User.Identity.GetUserId();
                restaurant.IsOpen = true;
                db.Restaurants.Add(restaurant);
                db.SaveChanges();
                return RedirectToAction("Index", "Restaurant");
            }

            ViewBag.StateId = new SelectList(db.States, "StateId", "Abbreviation");
            return View(restaurant);
        }

        public ActionResult DuplicateAddress()
        {
            return View();
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
        public ActionResult Edit(int? restaurantId)
        {
            if (restaurantId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var restaurant = db.Restaurants
                .Include(r => r.Address)
                .Include(r => r.Address.City)
                .Include(r => r.Address.State)
                .Include(r => r.Address.ZipCode)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();

            if (restaurant == null)
            {
                return HttpNotFound();
            }
            ViewBag.StateId = new SelectList(db.States, "StateId", "Abbreviation");
            return View(restaurant);
        }

        // POST: Restaurant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "RestaurantId,RestaurantName,AddressId,OpenTime,CloseTime,IsOpen,PeopleBeforeWarning,GracePeriodMinutes,CurrentWaitMinutes")] Restaurant restaurant)
        public ActionResult Edit(int restaurantId, string RestaurantName, string streetOne, string streetTwo, string city, string StateId, string zipCode, string lat, string lng, string openTime, string closeTime, bool IsOpen, int PeopleBeforeWarning, int GracePeriodMinutes)
        {
            //NOTE: FOR DEMO PURPOSES, THE EDIT ACTION WILL NOT SEARCH FOR A NEW LAT AN LNG IF THE ADDRESS CHANGES!
            //WHEN DEMONSTRATING THE APP, DO NOT CHANGE THE ADDRESS!

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("RestaurantManager"))
            {
                return HttpNotFound();
            }

            var restaurant = db.Restaurants
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();

            if (ModelState.IsValid)
            {
                //Again, not doing the address stuff for the Edit action:
                //Address address = GetAddress(streetOne, city, StateId, zipCode, lat, lng);
                //if (address.AddressId == 0)
                //{
                //    return RedirectToAction("DuplicateAddress", "Restaurant");
                //}

                restaurant.RestaurantName = RestaurantName;

                //restaurant.AddressId = address.AddressId;
                //restaurant.UserId = User.Identity.GetUserId();

                restaurant.OpenTime = openTime;
                restaurant.CloseTime = closeTime;
                restaurant.IsOpen = IsOpen;
                restaurant.PeopleBeforeWarning = PeopleBeforeWarning;
                restaurant.GracePeriodMinutes = GracePeriodMinutes;
                db.SaveChanges();
                return RedirectToAction("Index", "Restaurant");
            }

            ViewBag.StateId = new SelectList(db.States, "StateId", "Abbreviation");
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
                .Include(r => r.PendingEmployees)
                .Include(r => r.ConfirmedEmployees)
                .ToList();
            List<Restaurant> myRestaurants = new List<Restaurant>();
            string userId = User.Identity.GetUserId();
            var user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            foreach (Restaurant restaurant in allRestaurants)
            {
                if ( restaurant.ConfirmedEmployees != null )
                {
                    if (restaurant.ConfirmedEmployees.Contains(user))
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
                .Include(r => r.ConfirmedEmployees)
                .Include(r => r.PendingEmployees)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();
            var userId = User.Identity.GetUserId();
            var user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            restaurant.PendingEmployees.Add(user);
            db.SaveChanges();
            return RedirectToAction("DisplayMyRestaurants", "Restaurant");
        }

        public ActionResult ConfirmEmployees(int restaurantId)
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
                .Include(r => r.PendingEmployees)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();
            return View(restaurant);
        }

        public ActionResult ConfirmEmployee(int restaurantId, string userId)
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
                .Include(r => r.PendingEmployees)
                .Include(r => r.ConfirmedEmployees)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();
            var user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            restaurant.PendingEmployees.Remove(user);
            restaurant.ConfirmedEmployees.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index", "Restaurant");
        }

        public ActionResult DenyEmployee(int restaurantId, string userId)
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
                .Include(r => r.PendingEmployees)
                .Include(r => r.ConfirmedEmployees)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();
            var user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            restaurant.PendingEmployees.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index", "Restaurant");
        }

        public ActionResult ManageEmployees(int restaurantId)
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
                .Include(r => r.ConfirmedEmployees)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();
            return View(restaurant);
        }

        public ActionResult DeleteEmployee(int restaurantId, string userId)
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
                .Include(r => r.PendingEmployees)
                .Include(r => r.ConfirmedEmployees)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();
            var user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            restaurant.ConfirmedEmployees.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index", "Restaurant");
        }

        public ActionResult SetWaitMinutes(int currentWaitMinutes, int restaurantId)
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
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();
            restaurant.CurrentWaitMinutes = currentWaitMinutes;
            db.SaveChanges();
            TempData["tableVisitMessage"] = "Wait time updated to " + currentWaitMinutes + " minutes.";
            return RedirectToAction("Index", "TableVisit", new { restaurantId = restaurantId } );
        }

        public ActionResult ActivateRestaurant(int restaurantId)
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
                .Include(r => r.PendingEmployees)
                .Include(r => r.ConfirmedEmployees)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();

            return View(restaurant);
        }

        public ActionResult DinerConfirmation(int restaurantId)
        {
            var restaurant = db.Restaurants
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();
            return View(restaurant);
        }

 /*       public ActionResult DisplayAnalytics(int restaurantId)
        {
            var restaurant = db.Restaurants
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();

            var tableVisits = db.TableVisits
                .Where(t => t.RestaurantId == restaurantId)
                .OrderByDescending(t=> t.CreatedOn)
                .ToList();

            RestaurantDisplayAnalyticsViewModel viewModel = new RestaurantDisplayAnalyticsViewModel();
            viewModel.Restaurant = restaurant;
            viewModel.TableVisits = tableVisits;

            List<TableVisitColumn> tableVisitColumns = new List<TableVisitColumn>();
            TableVisitColumn tableVisitColumn;
            for (int i=tableVisits.Count-1; i>0; i--)
            {
                if ( i == tableVisits.Count-1 )
                {
                    tableVisitColumn = new TableVisitColumn();
                    tableVisitColumn.Date = tableVisits[i].CreatedOn.Date;
                    tableVisitColumn.TotalVisits++;
                    if ( tableVisits[i].IsHostEntry )
                    {
                        tableVisitColumn.HostEnteredVisits++;
                    }
                    else
                    {
                        tableVisitColumn.DinerEnteredVisits++;
                    }
                }

            }

            return View(viewModel);
        }

*/
    }
}
