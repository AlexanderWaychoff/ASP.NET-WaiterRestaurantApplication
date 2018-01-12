using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public DateTime CreatedOn { get; set; }
        public double AmountPaid { get; set; }
    }
}