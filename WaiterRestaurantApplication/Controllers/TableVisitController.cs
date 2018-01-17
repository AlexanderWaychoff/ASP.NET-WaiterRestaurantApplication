using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WaiterRestaurantApplication.Models;

namespace WaiterRestaurantApplication.Controllers
{
    public class TableVisitController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private MessengerController messenger = new MessengerController();

        // GET: TableVisit
        public ActionResult Index(int? restaurantId)
        {
            if (restaurantId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var restaurant = db.Restaurants
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();
            
            var tableVisits = db.TableVisits
                .Where(t => t.RestaurantId == restaurantId)
                .Where(t => t.IsActive == true)
                .OrderBy(t => t.CreatedOn)
                .ToList();

            TableVisitIndexViewModel viewModel = new TableVisitIndexViewModel();
            viewModel.Restaurant = restaurant;
            viewModel.TableVisits = tableVisits;

            if (TempData["tableVisitMessage"] != null)
            {
                ViewBag.infoMessage = TempData["tableVisitMessage"].ToString();
            }
            return View(viewModel);
        }

        // GET: TableVisit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableVisit tableVisit = db.TableVisits.Find(id);
            if (tableVisit == null)
            {
                return HttpNotFound();
            }
            return View(tableVisit);
        }

        // GET: TableVisit/Create
        public ActionResult Create(int restaurantId, bool isHostEntry)
        {
            var restaurant = db.Restaurants
                .Include(r => r.TableVisits)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();

            TableVisitCreateModel viewModel = new TableVisitCreateModel();
            viewModel.Restaurant = restaurant;
            viewModel.IsHostEntry = isHostEntry;

            return View(viewModel);
        }

        // POST: TableVisit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "TableVisitId,DinerName,DinerPhone,CreatedOn,WaitMinutes,WeatherConditionId,IsHostEntry,IsSatisfied,PartySize,IsWarned,GracePeriodStart,IsNoShow,IsActive")] TableVisit tableVisit)
        public ActionResult Create(int restaurantId, bool isHostEntry, string dinerName, string dinerPhone, int partySize)
        {

            //TO DO: ADD VALIDATION USING THE IF STATEMENT BELOW:
            //if (ModelState.IsValid)
            //{
            //}

            var restaurant = db.Restaurants
                .Include(r => r.TableVisits)
                .Include(r => r.Address.City)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();

            string cityName = restaurant.Address.City.Name;
            WeatherCondition weatherCondition = new WeatherCondition(cityName);

            TableVisit tableVisit = new TableVisit();
            tableVisit.DinerName = dinerName;
            tableVisit.DinerPhone = dinerPhone;
            tableVisit.CreatedOn = DateTime.Now;
            tableVisit.WeatherCondition = weatherCondition;
            tableVisit.IsHostEntry = isHostEntry;
            tableVisit.PartySize = partySize;
            tableVisit.IsWarned = false;
            tableVisit.IsNoShow = false;
            tableVisit.IsActive = true;
            tableVisit.RestaurantId = restaurantId;
            db.TableVisits.Add(tableVisit);
            db.SaveChanges();

            restaurant.TableVisits.Add(tableVisit);
            db.SaveChanges();
            if(isHostEntry)
            {
                TempData["tableVisitMessage"] = "Added " + dinerName + " to the wait list.";
                return RedirectToAction("Index", "TableVisit", new { restaurantId = restaurantId });
            }
            return RedirectToAction("DinerConfirmation", "Restaurant", new { restaurantId = restaurantId });
        }

        // GET: TableVisit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableVisit tableVisit = db.TableVisits.Find(id);
            if (tableVisit == null)
            {
                return HttpNotFound();
            }
            ViewBag.WeatherConditionId = new SelectList(db.WeatherConditions, "WeatherConditionId", "WeatherDescription", tableVisit.WeatherConditionId);
            return View(tableVisit);
        }

        // POST: TableVisit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TableVisitId,DinerName,DinerPhone,CreatedOn,WaitMinutes,WeatherConditionId,IsHostEntry,IsSatisfied,PartySize,IsWarned,GracePeriodStart,IsNoShow,IsActive")] TableVisit tableVisit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tableVisit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WeatherConditionId = new SelectList(db.WeatherConditions, "WeatherConditionId", "WeatherDescription", tableVisit.WeatherConditionId);
            return View(tableVisit);
        }

        // GET: TableVisit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableVisit tableVisit = db.TableVisits.Find(id);
            if (tableVisit == null)
            {
                return HttpNotFound();
            }
            return View(tableVisit);
        }

        // POST: TableVisit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TableVisit tableVisit = db.TableVisits.Find(id);
            db.TableVisits.Remove(tableVisit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult SendTableReadyNotification(int TableVisitId)
        {
            TableVisit currentTableVisit = db.TableVisits.Find(TableVisitId);
            messenger.SendSMSMessage(currentTableVisit.DinerPhone, "Your Table is ready!");
            currentTableVisit.GracePeriodStart = DateTime.Now;
            ViewBag.infoMessage = "Your Table Notification has been sent.";
            return RedirectToAction("Index", "TableVisit", new { restaurantId = currentTableVisit.RestaurantId });

        }

        private void WarnDiner(int TableVisitId)
        {
            TableVisit currentTableVisit = db.TableVisits.Find(TableVisitId);
            messenger.SendSMSMessage(currentTableVisit.DinerPhone, "Your Table will be ready Soon!" );
            currentTableVisit.IsWarned = true;

            //return RedirectToAction("Index","TableVisit", new { restaurantId = currentTableVisit.RestaurantId });
        }

        private int calculateWaitTime(TableVisit currentTableVisit)
        {

            DateTime gracePeriod = currentTableVisit.GracePeriodStart.Value;

            return (int) (gracePeriod.Subtract(currentTableVisit.CreatedOn).TotalMinutes);
        }

        public ActionResult HostRemoveFromLine(int tableVisitId)
        {
            TableVisit currentTableVisit = db.TableVisits.Find(tableVisitId);
            int currentRestaurantId = currentTableVisit.RestaurantId;
            messenger.SendSMSMessage(currentTableVisit.DinerPhone, "Thank you for using Waiter. Did you enjoy our service. (Type 'y' for yes or 'n' for no)");
            ViewBag.infoMessage = "You have Removed " + currentTableVisit.DinerName + "'s reservation.";
            currentTableVisit.IsActive = false;
            db.SaveChanges();
            return RedirectToAction("Index", "TableVisit", new { restaurantId = currentRestaurantId });
        }

        public ActionResult RemoveFromLine(TableVisit currentTableVisit)
        {
            int currentRestaurantId = currentTableVisit.RestaurantId;
            messenger.SendSMSMessage(currentTableVisit.DinerPhone, "Thank you for using Waiter. Did you enjoy our service. (Type 'y' for yes or 'n' for no)");
            currentTableVisit.IsActive = false;
            db.SaveChanges();
            ViewBag.infoMessage = currentTableVisit.DinerName + "'s reservation has been removed due to grace period reservation.";
            return RedirectToAction("Index", "TableVisit", new { restaurantId = currentTableVisit.RestaurantId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void ManageTableVisitTime()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);

            var timer = new System.Threading.Timer((e) =>
            {
                foreach (var resturant in db.Restaurants)
                {

                    Parallel.Invoke(
                        () =>CheckForDinersToWarn(resturant.TableVisits, resturant),
                        () =>CheckForExpiredGracePeriod(resturant.TableVisits, resturant)
                    );
                }
            }, null, startTimeSpan, periodTimeSpan);
        }

        private void CheckForDinersToWarn(List<TableVisit> tableVisits, Restaurant resturant)
        {
            int peopleInLine = 0;
            for(int i =0; i < tableVisits.Count; i++)
            {
                if(!tableVisits[i].IsWarned && tableVisits[i].IsActive)
                {
                    peopleInLine += tableVisits[i].PartySize;
                    if(peopleInLine >= resturant.PeopleBeforeWarning)
                    {
                        WarnDiner(tableVisits[i+1].TableVisitId);
                        break;
                    }
                }
            }
        }

        private void CheckForExpiredGracePeriod(List<TableVisit> tableVisits, Restaurant resturant)
        {
            for(int i=0;i< tableVisits.Count; i++)
            {
                if(tableVisits[i].IsActive && tableVisits[i].GracePeriodStart != null)
                {
                    DateTime gracePeriodStart = tableVisits[i].GracePeriodStart.Value;
                    if (gracePeriodStart.AddMinutes(resturant.GracePeriodMinutes) == DateTime.Now)
                    {
                        messenger.SendSMSMessage(tableVisits[i].DinerPhone, "Your Grace period has expired. You have lost your reservation.");
                        tableVisits[i].IsActive = false;
                        tableVisits[i].IsNoShow = true;
                        RemoveFromLine(tableVisits[i]);
                    }
                }
            }
        }
    }
}
