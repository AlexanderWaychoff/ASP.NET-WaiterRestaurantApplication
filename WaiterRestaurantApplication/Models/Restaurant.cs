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

        public Address AddressId { get; set; }
        public Address Address { get; set; }

        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public bool IsOpen { get; set; }

        public int TableVisitsId { get; set; }
        public List<TableVisit> TableVisits { get; set; }
        public int PeopleInLineBeforeWarning { get; set; }
        public int GracePeriodMinutes { get; set; }

        //public int EmployeeId { get; set; }
        //public ICollection<Employee> Employees { get; set; }

        //public int TransactionId {get; set; }
        //public ICollection<Transaction> Transactions { get; set; }
        public Subscription Subscription { get; set; }
        public int RestaurantManagerId { get; set; }
    }
}