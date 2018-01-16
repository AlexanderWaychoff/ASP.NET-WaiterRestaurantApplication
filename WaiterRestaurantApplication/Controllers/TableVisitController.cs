﻿using System;
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
        public ActionResult Create(int restaurantId)
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
                .Include(r => r.TableVisits)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();
            return View(restaurant);
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

            //Alex...add your magic below...
            string cityName = restaurant.Address.City.Name;
            //need to get weather condition first - It cannot be nullable
            //dummy weather condition:
            WeatherCondition weatherCondition = new WeatherCondition(cityName);
            //weatherCondition.Temperature = 75;
            //weatherCondition.WeatherDescription = "Partly Cloudy";


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
            TempData["tableVisitMessage"] = "Added " + dinerName + " to the wait list.";
            return RedirectToAction("Index", "TableVisit", new { restaurantId = restaurantId});
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


        public ActionResult SendTableReadyNotification(TableVisit currentTableVisit)
        {

            messenger.SendSMSMessage(currentTableVisit.DinerPhone, "Your Table is ready!");
            currentTableVisit.GracePeriodStart = DateTime.Now;
            currentTableVisit.WaitMinutes = calculateWaitTime(currentTableVisit);

            return RedirectToAction("Index");

        }

        private DateTime calculateWaitTime(TableVisit currentTableVisit)
        {
            return ((System.Math.Abs(currentTableVisit.GracePeriodStart.Subtract(currentTableVisit.CreatedOn)))/60);
        }
        public ActionResult RemoveFromLine(TableVisit currentTableVisit)
        {
            messenger.SendSMSMessage(currentTableVisit.DinerPhone, "Your Grace period has expired. You have lost your reservation.");
            db.TableVisits.Remove(currentTableVisit);
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
    }
}
