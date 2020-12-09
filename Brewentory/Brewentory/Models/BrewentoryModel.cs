using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Brewentory.Models 
{
    public class BrewentoryModel
    {
        // Inventory fields
        public int LocationID { get; set; }
        public string Location { get; set; }
        public string Product { get; set; }
        public string Quantity { get; set; }        

        // LiveView fields
        public int ProductID { get; set; }
        public string ProductLive { get; set; }
        public string Batch { get; set; }
        public int? Pallets { get; set; }
        public int? QuantityLive { get; set; }
        public bool LiveStatus { get; set; } 

        // CompletedWork fields
        public int completedWorkID { get; set; }
        public string Date { get; set; }
        public string cwProduct { get; set; }
        public string cwBatch { get; set; }
        public int? cwPallets { get; set; }
        public int? cwQuantity { get; set; }
        public string StartShift { get; set; }
        public string EndShift { get; set; }
        public int? Loss { get; set; }

        // Note fields
        public int HeadlineID { get; set; }
        public string Headline { get; set; }
        public string Note { get; set; }

        // Timesheet fields
        public int SheetID { get; set; }
        public int Week { get; set; }
        public string Name { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }


        // Shifts
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string ShiftTimes { get; set; }

        // Icons

        public string EditIcon { get; set; }
        public string DeleteIcon { get; set; }

        public bool BtnVisibility { get; set; }

        // Employees
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Phone { get; set; } 
        public string Password { get; set; }
        public bool LoggedIn { get; set; }

        // Shared 
        public string Operation { get; set; }
        public int WeekNo { get; set; }
        public bool VisibleOrNot { get; set; } 
        public string User { get; set; }

    }
}