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
    }
}