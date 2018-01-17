using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication.Models
{
    public class RestaurantIndexViewModel
    {
        public ICollection<Restaurant> ActiveRestaurants { get; set; }
        public ICollection<Restaurant> InactiveRestaurants { get; set; }
    }

    public class RestaurantDisplayAnalyticsViewModel
    {
        public Restaurant Restaurant { get; set; }
        public ICollection<TableVisit> TableVisits { get; set; }
        public ICollection<TableVisitColumn> TableVisitColumns { get; set; }
    }
}