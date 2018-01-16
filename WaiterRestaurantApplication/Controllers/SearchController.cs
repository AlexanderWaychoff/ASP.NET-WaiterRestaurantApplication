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
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Search
        public ActionResult Index()
        {
            var restaurants = db.Restaurants
                .Include(r => r.Address)
                .ToList();

            ViewBag.StateId = new SelectList(db.States, "StateId", "Abbreviation");

            return View(restaurants);
        }
    }
}