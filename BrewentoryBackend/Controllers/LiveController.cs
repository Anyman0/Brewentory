using Brewentory.Models;
using BrewentoryBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BrewentoryBackend.Controllers
{
    public class LiveController : ApiController
    {
        // GET: api/live
        public string[] GetAll()
        {
            string[] liveNow = null;
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {
                liveNow = (from l in entities.LiveViews select l.ProductID + ", " + l.Product + ", " + l.Batch + ", " + l.Pallets + ", " + l.Quantity + ", " + l.LiveStatus).ToArray();
            }
            finally
            {
                entities.Dispose();
            }

            return liveNow;
        }

        // GET: api/live?productID=""
        public BrewentoryModel GetModel(int productID)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            try
            {
                LiveView lv = (from l in entities.LiveViews where (l.ProductID == productID) select l).FirstOrDefault();
                BrewentoryModel chosenItem = new BrewentoryModel()
                {
                    ProductID = lv.ProductID,
                    ProductLive = lv.Product,
                    Batch = lv.Batch,
                    Pallets = lv.Pallets,
                    QuantityLive = lv.Quantity,
                    LiveStatus = lv.LiveStatus
                };

                return chosenItem;
            }
            finally
            {
                entities.Dispose();
            }
        }

        [HttpPost]
        public bool PostStatus(BrewentoryModel model)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {
                if(model.Operation == "Edit")
                {
                    LiveView existing = (from l in entities.LiveViews where (l.ProductID == model.ProductID) select l).FirstOrDefault();
                    if (existing != null)
                    {
                        existing.Product = model.ProductLive;
                        existing.Batch = model.Batch;
                        existing.Pallets = model.Pallets;
                        existing.Quantity = model.QuantityLive;
                        existing.LiveStatus = model.LiveStatus;
                    }
                    else return false;
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
    }
}
