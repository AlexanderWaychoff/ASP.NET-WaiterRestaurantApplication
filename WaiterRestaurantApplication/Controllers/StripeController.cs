using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaiterRestaurantApplication.Models;
using System.Data.Entity;

namespace WaiterRestaurantApplication.Controllers
{
    public class StripeController : Controller
    {

        //NOTE: THIS SHOULD BE MOVED INTO A PLAIN OLD CLASS, NOT A CONTROLLER!

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Stripe
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult ChargeMonthly(string stripeEmail, string stripeToken, int restaurantId)
        {
            DoStripeCharge(stripeEmail, stripeToken, 999, "Monthly Subscription");

            var restaurant = db.Restaurants
                .Include(r => r.Subscription)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();

            //TO DO: Refactor...code is very similar to Charge Annual method
            restaurant.Subscription = new Subscription();
            restaurant.Subscription.IsActive = true;
            restaurant.Subscription.SubscriptionTypeId = 1; //MONTHLY SUBSCRIPTION TYPE. TO DO: FIGURE OUT A WAY TO AVOID MAGIC NUMBER
            restaurant.Subscription.StartDate = DateTime.Now;
            restaurant.Subscription.EndDate = DateTime.Now.AddMonths(1);
            restaurant.Subscription.AutoRenewal = true; //TO DO: FIGURE OUT A WAY TO USE THE STRIPE API TO CANCEL SUBSCRIPTIONS! DOCUMENTATION DOESN'T SEEM TO INCLUDE ASP.NET INSTRUCTIONS
            db.SaveChanges();

            return RedirectToAction("Index", "Subscription", new { restaurantId = restaurantId });
        }

        public ActionResult ChargeAnnual(string stripeEmail, string stripeToken, int restaurantId)
        {
            DoStripeCharge(stripeEmail, stripeToken, 11000, "Annual Subscription");

            var restaurant = db.Restaurants
                .Include(r => r.Subscription)
                .Where(r => r.RestaurantId == restaurantId)
                .FirstOrDefault();

            //TO DO: Refactor...code is very similar to Charge Monthly method
            restaurant.Subscription = new Subscription();
            restaurant.Subscription.IsActive = true;
            restaurant.Subscription.SubscriptionTypeId = 2; //ANNUAL SUBSCRIPTION TYPE. TO DO: FIGURE OUT A WAY TO AVOID MAGIC NUMBER
            restaurant.Subscription.StartDate = DateTime.Now;
            restaurant.Subscription.EndDate = DateTime.Now.AddYears(1);
            restaurant.Subscription.AutoRenewal = true; //TO DO: FIGURE OUT A WAY TO USE THE STRIPE API TO CANCEL SUBSCRIPTIONS! DOCUMENTATION DOESN'T SEEM TO INCLUDE ASP.NET INSTRUCTIONS
            db.SaveChanges();

            return RedirectToAction("Index", "Subscription", new { restaurantId = restaurantId });
        }

        private void DoStripeCharge(string stripeEmail, string stripeToken, int amountToCharge, string description)
        {
            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();

            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            var charge = charges.Create(new StripeChargeCreateOptions
            {
                Amount = amountToCharge,
                Description = description,
                Currency = "usd",
                CustomerId = customer.Id
            });
        }

    }
}