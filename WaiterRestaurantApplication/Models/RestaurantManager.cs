using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication.Models
{
    public class RestaurantManager :ApplicationUser
    {
        public int RestaurantManagerId { get; set; }
        public ICollection<Restaurant> Restaurants { get; set; }
    }
}