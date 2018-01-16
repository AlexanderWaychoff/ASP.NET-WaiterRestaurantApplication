using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication.Models
{
    public class SubscriptionType
    {
        public int SubscriptionTypeId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}