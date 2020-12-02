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
    }
}
