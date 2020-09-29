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
    public class ShiftsController : ApiController
    {
        // GET: api/Shifts
        public IEnumerable<string> Get()
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            string[] shifts = null;

            try
            {
                shifts = (from s in entities.Timesheets select s.Week + ", " + s.Name + ", " + s.Monday + ", " + s.Tuesday + ", " + s.Wednesday + ", " + s.Thursday + ", " + s.Friday).ToArray();
            }
            finally
            {
                entities.Dispose();
            }

            return shifts;
        }

        // GET: api/Shifts/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Shifts
        [HttpPost]
        public void Post([FromBody]BrewentoryModel model)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            try
            {
                if(model.Operation == "Create")
                {
                    Shift shift = (from s in entities.Shifts where (s.ShiftName == model.ShiftName) select s).FirstOrDefault();
                    string shiftTime = shift.ShiftTimes;
                    Timesheet newEntry = new Timesheet()
                    {                       
                        Name = model.Name,
                        Monday = shiftTime,
                        Tuesday = shiftTime,
                        Wednesday = shiftTime,
                        Thursday = shiftTime,
                        Friday = shiftTime
                    };

                    entities.Timesheets.Add(newEntry);
                }

                entities.SaveChanges();
            }
            catch
            {

            }
            finally
            {
                entities.Dispose();
            }            
        }

        // PUT: api/Shifts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Shifts/5
        public void Delete(int id)
        {
        }
    }
}
