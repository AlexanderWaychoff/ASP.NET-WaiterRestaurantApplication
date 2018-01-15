using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication.Models
{
    public class TableVisit
    {
        public int TableVisitId { get; set; }
        public string DinerName { get; set; }
        public string DinerPhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public int WaitMinutes { get; set; }

        public WeatherCondition WeatherCondition { get; set; }
        public int? WeatherConditionId { get; set; }

        public bool IsHostEntry { get; set; }
        public bool? IsSatisfied { get; set; }
        public int PartySize { get; set; }
        public bool IsWarned { get; set; }
        public DateTime? GracePeriodStart { get; set; }
        public bool? IsNoShow { get; set; }
        public bool IsActive { get; set; }

        public int RestaurantId { get; set; }

    }
}