using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrewentoryBackend.Models
{
    public class BrewentoryModel
    {
        // Inventory fields
        public int LocationID { get; set; }
        public string Location { get; set; }
        public string Product { get; set; }
        public string Quantity { get; set; }

        // Note fields
        public int HeadlineID { get; set; }
        public string Headline { get; set; }
        public string Note { get; set; }

        // Timesheet fields
        public int EmployeeID { get; set; }
        public string Week { get; set; }
        public string Name { get; set; }

        // Shifts
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string ShiftTimes { get; set; }

    }
}