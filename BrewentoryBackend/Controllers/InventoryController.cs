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
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace BrewentoryBackend.Controllers
{
    public class InventoryController : ApiController
    {
                     
        // Get api/inventory    FINALLY WORKS
        public string[] GetAll()
        {
            string[] inventory = null;
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {
                inventory = (from iv in entities.Inventories where (iv.Location != null) select iv.LocationID + ", " + iv.Location + ", "  + iv.Product + ", " + iv.Quantity).ToArray();
            }
            finally
            {
                entities.Dispose();
            }

            return inventory;
           
        }

         // Get api/inventory?item=""
        public BrewentoryModel GetModel(/*string item*/int locationID)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {
                //string chosenItem = item;
                int locID = locationID;
                Inventory inventory = (from iv in entities.Inventories where (iv.Location != null) && (iv.LocationID == locID) select iv).FirstOrDefault();

                BrewentoryModel chosenItemModel = new BrewentoryModel()
                {
                    LocationID = inventory.LocationID,
                    Location = inventory.Location,
                    Product = inventory.Product,
                    Quantity = inventory.Quantity
                };

                return chosenItemModel;
            }
            finally
            {
                entities.Dispose();
            }
        }

        [HttpPost]
        public bool PostInventory(BrewentoryModel model)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {

                if(model.Operation == "Create")
                {
                    Inventory newEntry = new Inventory()
                    {
                        Location = model.Location,
                        Product = model.Product,
                        Quantity = model.Quantity
                    };

                    entities.Inventories.Add(newEntry);
                }

                else if(model.Operation == "Edit")
                {
                    Inventory existing = (from i in entities.Inventories where (i.LocationID == model.LocationID) select i).FirstOrDefault();
                    if(existing != null)
                    {
                        existing.Location = model.Location;
                        existing.Product = model.Product;
                        existing.Quantity = model.Quantity;
                    }                    
                    else
                    {
                        return false;
                    }
                }

                else if(model.Operation == "Delete")
                {
                    Inventory existing = (from i in entities.Inventories where (i.LocationID == model.LocationID) select i).FirstOrDefault();
                    if(existing != null)
                    {
                        entities.Inventories.Remove(existing);
                    }
                    else
                    {
                        return false;
                    }
                }

                entities.SaveChanges();
            }
            catch
            {
                return false;
            }
            finally
            {
                entities.Dispose();
            }

            return true;
        }

        
        public bool PostStatus(string value)
        {
            return true;
        }

    }

   
}
