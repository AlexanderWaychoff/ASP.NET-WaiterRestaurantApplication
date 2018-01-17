using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WaiterRestaurantApplication.Controllers;

namespace WaiterRestaurantApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        TableVisitController tablevisits = new TableVisitController();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            /*
            var timer = new System.Threading.Timer((e) =>
            {
                tablevisits.ManageTableVisitTime();
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            */
        }
    }
}
