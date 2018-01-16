using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }

        public SubscriptionType SubscriptionType { get; set; }
        public int SubscriptionTypeId { get; set; }

        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool AutoRenewal { get; set; }
    }
}