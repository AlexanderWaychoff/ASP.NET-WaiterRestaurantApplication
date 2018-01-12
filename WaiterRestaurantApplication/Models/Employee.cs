using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication.Models
{
    public class Employee : ApplicationUser
    {
        public int EmployeeId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}