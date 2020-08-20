using Brewentory.Models;
using BrewentoryBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;

namespace BrewentoryBackend.Controllers
{
    public class InventoryController : ApiController
    {
             
        [HttpGet]
        // Get api/inventory    FINALLY WORKS
        public string[] GetAll()
        {
            string[] inventory = null;
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {
                inventory = (from iv in entities.Inventories where (iv.Location != null) select iv.Location + ", "  + iv.Product + ", " + iv.Quantity).ToArray();
            }
            finally
            {
                entities.Dispose();
            }

            return inventory;
           
        }


        public BrewentoryModel GetModel(string inven)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {
                string inv = inven;
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
