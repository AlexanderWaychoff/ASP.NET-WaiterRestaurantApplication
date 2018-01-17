using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication
{
    public class TableVisitColumn
    {
        public DateTime Date { get; set; }
        public int TotalVisits { get; set; }
        public int HostEnteredVisits { get; set; }
        public int DinerEnteredVisits { get; set; }
    }
}