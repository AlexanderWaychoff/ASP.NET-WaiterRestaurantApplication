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
    public class TableVisitController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TableVisit
        public ActionResult Index()
        {
            var tableVisits = db.TableVisits.Include(t => t.WeatherCondition);
            return View(tableVisits.ToList());
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
        public ActionResult Create()
        {
            ViewBag.WeatherConditionId = new SelectList(db.WeatherConditions, "WeatherConditionId", "WeatherDescription");
            return View();
        }

        // POST: TableVisit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TableVisitId,DinerName,DinerPhone,CreatedOn,WaitMinutes,WeatherConditionId,IsHostEntry,IsSatisfied,PartySize,IsWarned,GracePeriodStart,IsNoShow,IsActive")] TableVisit tableVisit)
        {
            if (ModelState.IsValid)
            {
                db.TableVisits.Add(tableVisit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WeatherConditionId = new SelectList(db.WeatherConditions, "WeatherConditionId", "WeatherDescription", tableVisit.WeatherConditionId);
            return View(tableVisit);
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
