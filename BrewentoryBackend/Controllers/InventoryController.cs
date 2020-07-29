using BrewentoryBackend.DataAccess;
using BrewentoryBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace BrewentoryBackend.Controllers
{
    public class InventoryController : ApiController
    {
             
        public string[] GetAll()
        {
            string[] inventory = null;
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {
                inventory = (from iv in entities.Inventories where (iv.Location != null) select iv.Location + " " + iv.Product + " " + iv.Quantity ).ToArray();
            }
            finally
            {
                entities.Dispose();
            }

            return inventory;
        }


        public BrewentoryModel GetModel()
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {
                Inventory inventory = (from iv in entities.Inventories where (iv.Location != null) select iv).FirstOrDefault();
                BrewentoryModel model = new BrewentoryModel()
                {
                    Location = inventory.Location,
                    Product = inventory.Product,
                    Quantity = inventory.Quantity
                };

                return model;
            }
            finally
            {
                entities.Dispose();
            }
        }

    }

   
}
