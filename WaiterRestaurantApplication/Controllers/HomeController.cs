﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaiterRestaurantApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            AnalyticsController analytic = new AnalyticsController();
            analytic.CalculateEstimatedWaitTimes(1);

            return View();
        }
    }
}