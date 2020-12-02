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
    public class CompletedWorkApiController : ApiController
    {
        // Get api/completedworkapi 
        public string[] GetAll()
        {
            string[] completed = null;
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {
                completed = (from cw in entities.CompletedWorks select cw.CompletedWorkID + ", " + cw.Date + ", " + cw.Product + ", " + cw.Batch + ", " + cw.Pallets + ", " + cw.Quantity + ", " + cw.StartShift + ", " + cw.EndShift + ", " + cw.Loss).ToArray();
            }
            finally
            {
                entities.Dispose();
            }

            return completed;
        }

        // GET: api/completedworkapi?cwid=""
        public BrewentoryModel GetModel(int cwId)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            try
            {
                CompletedWork cw = (from c in entities.CompletedWorks where (c.CompletedWorkID == cwId) select c).FirstOrDefault();
                BrewentoryModel chosenItem = new BrewentoryModel()
                {
                    completedWorkID = cw.CompletedWorkID,
                    Date = cw.Date.ToShortDateString(),
                    cwProduct = cw.Product,
                    cwBatch = cw.Batch,
                    cwPallets = cw.Pallets,
                    cwQuantity = cw.Quantity,
                    StartShift = cw.StartShift,
                    EndShift = cw.EndShift,
                    Loss = cw.Loss
                };
                return chosenItem;
            }
            finally
            {
                entities.Dispose();
            }
        }

        [HttpPost] 
        public bool PostCompletedWork(BrewentoryModel model)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            try
            {
                if(model.Operation == "Edit")
                {
                    CompletedWork existing = (from c in entities.CompletedWorks where (c.CompletedWorkID == model.completedWorkID) select c).FirstOrDefault();
                    if (existing != null)
                    {
                        //existing.Date = DateTime.Now;
                        existing.Product = model.cwProduct;
                        existing.Batch = model.cwBatch;
                        existing.Pallets = model.cwPallets;
                        existing.Quantity = model.cwQuantity;
                        existing.StartShift = model.StartShift;
                        existing.EndShift = model.EndShift;
                        existing.Loss = model.Loss;
                    }
                    else return false;
                }
                else if(model.Operation == "Delete")
                {
                    CompletedWork existing = (from c in entities.CompletedWorks where (c.CompletedWorkID == model.completedWorkID) select c).FirstOrDefault();
                    if (existing != null)
                    {
                        entities.CompletedWorks.Remove(existing);
                    }
                    else return false;
                }
                else if(model.Operation == "DeleteAll")
                {
                    var cw = entities.CompletedWorks;
                    foreach(var completedWork in cw)
                    {
                        CompletedWork c = entities.CompletedWorks.Find(completedWork.CompletedWorkID);
                        entities.CompletedWorks.Remove(c);
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
    }
}
