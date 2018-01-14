using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }

        public Address Address { get; set; }
        public int AddressId { get; set; }

        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public bool IsOpen { get; set; }

        public List<TableVisit> TableVisits { get; set; }

        public int PeopleBeforeWarning { get; set; }
        public int GracePeriodMinutes { get; set; }

        public ICollection<ApplicationUser> PendingEmployees { get; set; }
        public ICollection<ApplicationUser> ConfirmedEmployees { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public Subscription Subscription { get; set; }
        public int CurrentWaitMinutes { get; set; }
        //public int AverageWaitMinutes = calculate WaitMinutes

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

    }
}