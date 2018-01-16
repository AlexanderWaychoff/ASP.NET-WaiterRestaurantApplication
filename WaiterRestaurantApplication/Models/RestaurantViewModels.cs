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
}