﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaiterRestaurantApplication.Models
{
    public class TableVisitIndexViewModel
    {
        public List<TableVisit> TableVisits { get; set; }
        public Restaurant Restaurant { get; set; }
    }

    public class TableVisitCreateModel
    {
        public Restaurant Restaurant { get; set; }
        public bool IsHostEntry { get; set; }
    }
}